using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lgxt
{
    internal class User
    {
        public UserInfo userInfo { get; set; }
        public List<Book> books { get; set; }
        public List<Work> works { get; set; }
    }

    internal class UserInfo
    {
        public int userId { get; set; }
        public int deptId { get; set; }
        public string deptName { get; set; }
        public string schoolName { get; set; }
        public string loginName { get; set; }
        public string userName { get; set; }
        public string studentNo { get; set; }
        public string email { get; set; }
        public string phonenumber { get; set; }
    }

    internal class Book
    {
        public int bookId { get; set; }
        public string bookName { get; set; }
        public int courseId { get; set; }
        public string courseName { get; set; }
        public object bookDescription { get; set; }
        public string coverUrl { get; set; }
        public object courseDescription { get; set; }
        public int teacherId { get; set; }
        public string teacherName { get; set; }
        public string courseType { get; set; }
    }


    internal class Work
    {
        public int workId { get; set; }
        public int chapterId { get; set; }
        public string workName { get; set; }
        public string chapterName { get; set; }
        public int times { get; set; }
        public int score { get; set; }
        public int questionNum { get; set; }
        public int tryTimes { get; set; }
        public int grade { get; set; }
        public object totalGrade { get; set; }
        public string expireTime { get; set; }
        public string workType { get; set; }
        public int testTime { get; set; }
        public int totalScore { get; set; }
    }

}


