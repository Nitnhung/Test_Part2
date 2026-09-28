using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PhanCongViec.Abstract;
using PhanCongViec.Data;
using PhanCongViec.DTOs;
using PhanCongViec.Models;
using System.ComponentModel.DataAnnotations;

namespace PhanCongViec.Implement
{

    public class WorkboardsService : IWorkboardService
    {
        private readonly AppDbContext _context;
        public WorkboardsService(AppDbContext context)
        {
            _context = context;
        }


        public async Task<bool> SoftDeletedAsync(long id)
        {
            var workItem = await _context.WorkItems.FirstOrDefaultAsync(i=>i.Id ==id);
            if (workItem is null || workItem.IsDeleted == true)
            {
                return false;
            }
            workItem.IsDeleted = true;
            workItem.DeletedAt = DateTime.UtcNow;
            workItem.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<DetailWorkItem> PostWorkItem(long id, string note)
        {
              if (note is null)
                {
                    return null;
                }
                if (id < 1) return null;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var noteHistory = await _context.AddAsync(note);
                    var result = await _context.WorkItems
                           .Include(x => x.Project)
                           .Include(x => x.Assignee)
                           .Include(x => x.Labels)
                           .Include(x => x.WorkItemHistories)
                           .FirstOrDefaultAsync(i => i.Id == id && i.IsDeleted == false);

                    if (result == null)
                    {
                        return null;
                    }

                    WorkItemHistory newNote = new WorkItemHistory();
                    newNote.Note = note;
                    var newHistory = new WorkItemHistoryDto
                    {
                        id = result.Id,
                        ChangedBy = "api",
                        CreatedAt = DateTime.UtcNow,
                        FromStatus = result.Status,
                        ToStatus = result.Status,
                        Note = newNote.Note
                    };
                    await _context.SaveChangesAsync();

                    var newObHistoryItem = new DetailWorkItem
                    {

                        Item = new WorkItemDto
                        {
                            Id = result.Id,
                            Code = result.Code,
                            Title = result.Title,
                            Description = result.Description,
                            Status = result.Status,
                            Priority = result.Priority,
                            DueAt = result.DueAt,
                            CreatedAt = result.CreatedAt,
                            UpdatedAt = result.UpdatedAt,
                            CompletedAt = result.CompletedAt
                        },

                        Project = new ProjectDto
                        {
                            Code = result.Project.Code,
                            Name = result.Project.Name
                        },

                        Assignee = new AssigneeDto
                        {
                            Code = result.Assignee.Code,
                            FullName = result.Assignee.FullName,
                            Id = result.Assignee.Id
                        },

                        Label = result.Labels
                            .OrderBy(x => x.Name)
                            .Select(x => x.Name)
                            .ToList(),

                        WorkItemHistories = result.WorkItemHistories
                            .OrderBy(cr => cr.CreatedAt)
                            .ThenBy(cr => cr.Id).
                            Select(g => new WorkItemHistoryDto
                            {
                                CreatedAt = g.CreatedAt,
                                ChangedBy = g.ChangedBy,
                                FromStatus = g.FromStatus,
                                ToStatus = g.ToStatus,
                                Note = g.Note
                            }).ToList()
                    };
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return newObHistoryItem;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"loi he thong {ex.Message}");
                }
            }
                
        }



        public async Task<DetailWorkItem?> GetDetailWorkItemByIdAsync(long id)
        {

            var result = await _context.WorkItems
                   .Include(x => x.Project)
                   .Include(x => x.Assignee)
                   .Include(x => x.Labels)
                   .Include(x => x.WorkItemHistories)
                   .FirstOrDefaultAsync(i => i.Id == id && i.IsDeleted == false);
            if (result == null)
            {
                return null;
            }

            return new DetailWorkItem
            {
                Item = new WorkItemDto
                {
                    Id = result.Id,
                    Code = result.Code,
                    Title = result.Title,
                    Description = result.Description,
                    Status = result.Status,
                    Priority = result.Priority,
                    DueAt = result.DueAt,
                    CreatedAt = result.CreatedAt,
                    UpdatedAt = result.UpdatedAt,
                    CompletedAt = result.CompletedAt
                },
                Project = new ProjectDto
                {
                    Code = result.Project.Code,
                    Name = result.Project.Name
                },
                Assignee = new AssigneeDto
                {
                    Code = result.Assignee.Code,
                    FullName = result.Assignee.FullName,
                    Id = result.Assignee.Id
                },
                Label = result.Labels.OrderBy(x => x.Name)
                .Select(x => x.Name).
                ToList(),
                WorkItemHistories = result.WorkItemHistories
                .OrderBy(cr => cr.CreatedAt)
                .ThenBy(cr => cr.Id).
                Select(g => new WorkItemHistoryDto
                {
                    CreatedAt = g.CreatedAt,
                    ChangedBy = g.ChangedBy,
                    FromStatus = g.FromStatus,
                    ToStatus = g.ToStatus,
                    Note = g.Note
                }).ToList()

            };

        }



        public async Task<WorkItemHistoryDto> GetHistoryDtoAsync(long id)
        {
            var result = await _context.WorkItemHistories.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null || result.Id < 1)
            {
                return null;
            }
            return new WorkItemHistoryDto
            {
                id = result.Id,
                FromStatus = result.FromStatus,
                ToStatus = result.ToStatus,
                Note = result.Note,
                ChangedBy = result.ChangedBy,
                CreatedAt = result.CreatedAt,
            };


        }



        public async Task<List<History>> GetHistoryByDateFormTo(DateTime? fromDate, DateTime? toDate)
        {
            if (fromDate.HasValue || toDate is not null)        return null;
            if (fromDate > toDate)                              return null;
            if (fromDate is null && toDate is null)             return null;
            
            var result = await _context.WorkItems
               .Join(_context.WorkItemHistories,
                w => w.Id,
                wh => wh.WorkItemId,
                (w, wh) => new { w, wh })
                .GroupBy(x => new { x.w.CreatedAt, x.wh.Id })
                .Select(g => new History
                {
                    CreatedAt = g.Key.CreatedAt,
                    Infor = g.Select(n => new WorkItemHistoryDto
                    {
                        ChangedBy = n.wh.ChangedBy,
                        CreatedAt = n.wh.CreatedAt,
                        FromStatus = n.wh.FromStatus,
                        ToStatus = n.wh.ToStatus,
                        id = n.w.Id,
                        Note = n.wh.Note
                    })
                .OrderBy(x => x.CreatedAt)
                .ThenBy(x => x.id)
                .ToList()
                })
                .ToListAsync();
            return result;
        }
    }
}
