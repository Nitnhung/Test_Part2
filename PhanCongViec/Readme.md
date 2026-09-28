HƯỚNG DÂN KẾT NỐI VỚI DATABASE 
bước 1: Vào `appsettings.json`, cấu hình `ConnectionStrings` như sau:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=10.8.0.1;Port=5432;Database=API;Username=postgres;Password=arPNmdJER6m42346;"
}
```

bước 2: Mở CÔNG CỤ QUẢN LÝ DATABASE nhập tài khoản và mật khẩu để kết nối tới server chứa database.

bước 3:  Mở lại Visual Studiovà chạy chương trình với bằng https.

bước 4:  Mở postman và nhập các endpoint sau:

Health Check để kiểm tra kết nối database:GET https://localhost:7180/api/workboards/HealthCheck
 nếu kết quả trả về là  như sau thì đã kết nối thành công:
 {
  "traceId": "...",
  "status": 200,
  "message": "success",
  "data": { "data": "true" }
}


