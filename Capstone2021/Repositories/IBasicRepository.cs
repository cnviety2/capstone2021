using System.Collections.Generic;

namespace Capstone2021.Repository
{
    /**
     * Interface này dùng để mô tả những action cơ bản crud với 1 table
     */
    interface IBasicRepository<T>
    {
        /// <summary>
        /// Lấy tất cả từ 1 table
        /// </summary>
        /// <returns></returns>
        IList<T> getAll();

        /// <summary>
        /// Lấy 1 record dựa trên primary key (id) của table đó
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T get(int id);

        /// <summary>
        /// Thêm mới 1 record vào trong table
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool create(T obj);

        /// <summary>
        /// Xóa 1 record trong db,xóa vĩnh viễn
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool remove(int id);

        /// <summary>
        /// Đổi status 1 record trong db,ko xóa vĩnh viễn
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool softRemove(int id);
    }
}
