using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone2021.DTO
{
    public class Job
    {
        public int id { get; set; }
        public string name { get; set; }
        /// <summary>
        /// Part time hay full time hay cả 2,1 cho pt,2 cho ft,3 cho cả 2
        /// </summary>
        public int workingForm { get; set; }
        /// <summary>
        /// quận:từ 1 tới 12,13 bình tân,14 bình thạnh,15 gò vấp,16 phú nhuận,17 tân bình
        /// ,18 tân phú,19 thủ đức,20 bình chánh,21 cần giờ,22 củ chi,23 hóc môn,24 nhà bè
        /// </summary>
        public int location { get; set; }

        /// <summary>
        /// Chi tiết địa chỉ nơi làm việc
        /// </summary>
        public string workingPlace { get; set; }
        public string description { get; set; }
        /// <summary>
        /// Những requirements tuyển dụng đưa ra,1 đoạn text
        /// </summary>
        public string requirement { get; set; }

        /// <summary>
        /// true là việc làm chuyên ngành,false là việc làm phổ thông
        /// </summary>
        public bool type { get; set; }

        /// <summary>
        /// offer của nhà tuyển dụng,1 đoạn text
        /// </summary>
        public string offer { get; set; }

        /// <summary>
        /// Yêu cầu giới tính,1 là nam,2 là nữ,3 là cả 2 giới
        /// </summary>
        public Int32 sex { get; set; }

        /// <summary>
        /// Số lượng cần tuyển
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// Mức lương offer từ min tới max,VD:2 000 000 tới 5 000 000
        /// </summary>
        public int salaryMin { get; set; }
        public int salaryMax { get; set; }

        /// <summary>
        /// Id của nhà tuyển dụng đăng tin
        /// </summary>
        public int recruiterId { get; set; }
        public String createDate { get; set; }

        /// <summary>
        /// Id của staff đã duyệt job 
        /// </summary>
        public Int32 managerId { get; set; }

        /// <summary>
        /// 1 là pending(đợi duyệt),2 là ok(đã duyệt),3 là edited(đã đc update,lúc này job sẽ ko thể update nữa)
        /// </summary>
        public int status { get; set; }

        [JsonIgnore]
        public ICollection<job_has_category> relationship { get; set; }

        [JsonIgnore]
        public ICollection<student_apply_job> relationShipWithStudent { get; set; }

        public IList<Category> categories { get; set; }

        /// <summary>
        /// Chuỗi tạo ra để thể hiện cho job này,dùng để kiểm tra suggest cho 1 student
        /// </summary>
        public string stringForSuggestion { get; set; }

        /// <summary>
        /// Kiểm tra xem Job này có chứa category cần tìm hay ko,method này dùng trong việc search những job theo category,trả về 
        /// true nếu đúng
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool hasCategory(int categoryId)
        {
            if (this.categories == null || this.categories.Count == 0)
            {
                return false;
            }
            foreach (Category element in this.categories)
            {
                if (element.code == categoryId)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Trả về trọng số của job này match với lastAppliedJobString của student,best case là 4,worst case là 0,suggesst sẽ lấy từ 2 đến 4
        /// </summary>
        /// <param name="lastAppliedJobString"></param>
        /// <returns></returns>
        public int weightCompareToLastAppliedJob(string lastAppliedJobString)
        {
            string thisJobString = this.stringForSuggestion;
            string[] studentLastAppliedJobArray = lastAppliedJobString.Split('-');
            string[] thisJobSuggestStringArray = thisJobString.Split('-');

            int matchingWeight = 0;//đếm xem có bao nhiêu dữ liệu là equal

            if (Int32.Parse(studentLastAppliedJobArray[0]) == Int32.Parse(thisJobSuggestStringArray[0]))//kiểm tra về salaryMin
            {
                matchingWeight = matchingWeight + 2;
            }

            int[] categoriesOfLastAppliedJob = studentLastAppliedJobArray[1].Split(';').Select(n => Convert.ToInt32(n)).ToArray();
            int[] categoriesOfThisJob = thisJobSuggestStringArray[1].Split(';').Select(n => Convert.ToInt32(n)).ToArray();

            int numberDuplicates = categoriesOfLastAppliedJob.Intersect(categoriesOfThisJob).Count();//tìm nếu có equal về category
            if (numberDuplicates > 0)
            {
                matchingWeight = matchingWeight + 1;
            }

            if (Int32.Parse(studentLastAppliedJobArray[2]) == Int32.Parse(thisJobSuggestStringArray[2]))
            {
                matchingWeight = matchingWeight + 1;
            }
            return matchingWeight;
        }

    }
}