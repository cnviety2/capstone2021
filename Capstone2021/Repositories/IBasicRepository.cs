using System.Collections.Generic;

namespace Capstone2021.Repository
{
    /**
     * Interface này dùng để mô tả những action cơ bản crud với 1 table
     */
    interface IBasicRepository<T>
    {
        /**
         * Lấy tất cả từ 1 table
         */
        IList<T> getAll();

        /**
         * Lấy 1 record dựa trên primary key (id) của table đó
         */
        T get(int id);

        /**
         * Thêm mới 1 record vào trong table
         */
        bool create(T obj);

        /**
         * Xóa 1 record trong bảng(hoặc đổi status hoặc remove vĩnh viễn) dựa trên primary key
         */
        bool remove(int id);

    }
}
