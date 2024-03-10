using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics.Tracing;
using System.Text;
using Microsoft.VisualBasic;
using System.Net;
using System.Data;
namespace lgxt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void loginButton_Click(object sender, EventArgs e)
        {

            string loginName = textBox1.Text;
            string password = textBox2.Text;

            this.authorization = await Login(loginName, password);


            if (authorization != null)
            {
                await GetUserInfo();
                await GetCourses();
                DataTable coursesTable = new DataTable();
                coursesTable.Columns.Add("课程id");
                coursesTable.Columns.Add("课程名称");
                foreach (var book in currentUser.books)
                {
                    coursesTable.Rows.Add(book.courseId, book.courseName);
                }
                dataGridView1.DataSource = coursesTable;
            }
            else
            {
                MessageBox.Show("登录失败，请检查账号或密码是否正确!");
            }

        }

        private async Task GetUserInfo()
        {
            using (var httpClient = new HttpClient())
            {
                var url = "http://lgxt.wutp.com.cn/api/userInfo";
                httpClient.DefaultRequestHeaders.Add("Authorization", authorization);
                var response = await httpClient.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonObj = JObject.Parse(responseBody);
                    currentUser.userInfo = JsonConvert.DeserializeObject<UserInfo>(jsonObj["data"].ToString());
                    MessageBox.Show($"您好，来自{currentUser.userInfo.deptName}的{currentUser.userInfo.userName}。");
                }
                else
                {
                    MessageBox.Show("获取用户数据失败，请稍后重试！");
                }
            }
        }

        private async Task<string?> Login(string loginName, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var url = "http://lgxt.wutp.com.cn/api/login";

                var payLoad = $"loginName={loginName}&password={password}&";

                var content = new StringContent(payLoad, Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    JObject jsonObj = JObject.Parse(responseBody);

                    return (string)jsonObj["data"];
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task GetCourses()
        {
            using (var httpClient = new HttpClient())
            {
                var url = "http://lgxt.wutp.com.cn/api/myCourses";
                httpClient.DefaultRequestHeaders.Add("Authorization", authorization);
                var response = await httpClient.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonObj = JObject.Parse(responseBody);
                    currentUser.books = JsonConvert.DeserializeObject<List<Book>>(jsonObj["data"].ToString());
                  
                }
                else
                {
                    MessageBox.Show("获取课程数据失败，请稍后重试！");
                }
            }
        }

        private async Task GetWorks(string courseId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = "http://lgxt.wutp.com.cn/api/myCourseWorks";
                httpClient.DefaultRequestHeaders.Add("Authorization", authorization);


                var payLoad = $"courseId={courseId}&";

                var content = new StringContent(payLoad, Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await httpClient.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonObj = JObject.Parse(responseBody);

                    if (jsonObj["code"].ToString() == "500")
                    {
                        MessageBox.Show(jsonObj["msg"].ToString());

                    }
                    else
                    {
                        currentUser.works = JsonConvert.DeserializeObject<List<Work>>(jsonObj["data"].ToString());
                        foreach (var work in currentUser.works)
                        {
                            string wordId = work.workId.ToString();
                            SubmitAnswer(wordId);
                        }
                        MessageBox.Show("提交完成！");

                        string loginName = textBox1.Text;
                        string password = textBox2.Text;

                        this.authorization = await Login(loginName, password);
                    }
                    
                }
                else
                {
                    MessageBox.Show("获取课程数据失败，请稍后重试！");
                }
            }
        }

        private async void SubmitAnswer(string workId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = "http://lgxt.wutp.com.cn/api/submitAnswer";
                httpClient.DefaultRequestHeaders.Add("Authorization", authorization);


                var payLoad = $"grade=100&workId={workId}&&";

                var content = new StringContent(payLoad, Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await httpClient.PostAsync(url, content);


                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    JObject jsonObj = JObject.Parse(responseBody);
                }
                else
                {
                    MessageBox.Show("提交数据失败，请稍后重试！");
                }
            }
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int rowIdx = e.RowIndex;
                string courseId = dataGridView1.Rows[rowIdx].Cells[0].Value.ToString();
                string courseName = dataGridView1.Rows[rowIdx].Cells[1].Value.ToString();
                await GetWorks(courseId);
                
            }
        }
        private User currentUser = new User();

        private string? authorization;

        
    }
}
