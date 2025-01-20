using Grpc.Core;
using ProjectWarrantlyRecordGrpcServer.Interface;
using System.Collections.Concurrent;


namespace ProjectWarrantlyRecordGrpcServer.MessageContext
{
    public class EmailQueue // Dùng để tạo hàng đợi và lấy dữ liệu từ hàng đợi
    {
        private readonly ConcurrentQueue<NotificationParameters> _queue = new();
        private readonly SemaphoreSlim _signal = new(0); //Dùng để đồng bộ hóa hoạt động giữa việc thêm công việc (enqueue) và lấy công việc (dequeue) trong hàng đợi
                                                         // = new(0) có nghĩa ban đầu k có tín hiệu chạy, không chạy và chờ tín hiệu tăng mới chạy
                                                         // Có thể tạo ra giới hạn số lượng tín hiệu xử lý => new SemaphoreSlim(initialCount: 0, maxCount: [số lượng giới hạn muốn]);
                                                         // ******* Khuyền nghị dựa trên tính chất phức tạp của công việc và dung lượng xử lý giới hạn xử lý số lượng tín hiệu cùng lúc và số lượng công việc trong hàng đợi tầm: 10- 20

        public int QueueCount => _queue.Count;

        public void Enqueue(NotificationParameters email) // Thêm công việc đặt email vào hàng đợi
        {
            if (email == null) 
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Thông tin email không được tạo xuống đây làm gì ???"));
            _queue.Enqueue(email);
            _signal.Release(); // Được gọi, tăng giá trị tín hiệu lên 1(Mặc định). Điều này báo hiệu rằng có công việc trong hàng đợi để xử lý.
                               // Nếu muốn tăng nhiều tín hiệu cùng lúc => _signal.Release([Số lượng muốn bắt]) 
        }

        //CancellationToken : Là một công cụ để báo hiệu việc hủy bỏ một tác vụ bất đồng bộ (asynchronous task).
        // Giúp hủy các tác vụ chờ mà không gây ra lỗi hoặc làm hỏng hàng đợi.
        public async Task<NotificationParameters> DequeueAsync(CancellationToken cancellationToken) // lấy công việc ra
        {
            await _signal.WaitAsync(cancellationToken); // Kiểm tra tín hiệu hiện tại dưới hệ thống:
                                                        // Nếu giá trị tín hiệu lớn hơn 0, nó sẽ giảm tín hiệu đi 1 và tiếp tục xử lý.
                                                        // Nếu tín hiệu bằng 0, luồng sẽ bị chặn cho đến khi có tín hiệu mới.
                                                        // WaitAsync không tốn CPU khi chờ tín hiệu (khác với các vòng lặp chờ bận).
                                                        // Muốn giảm nhiều tín hiệu cùng lúc bắt buộc phải viết nhiều lần => Vd có 3 tiến hiệu viết 3 lần await semaphore.WaitAsync(); 
            _queue.TryDequeue(out var email);
            return email;
        }
    }
}
