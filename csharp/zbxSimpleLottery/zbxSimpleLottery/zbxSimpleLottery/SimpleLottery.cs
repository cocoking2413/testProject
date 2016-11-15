using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Threading;
using DBContact;
using Lottery;



namespace zbxSimpleLottery
{
    public partial class SimpleLottery : Form
    {
        public SimpleLottery()
        {
            InitializeComponent();
          
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var num = new Random().Next(100).ToString();
            this.textBox1.Text = num;
            this.richTextBox1.Text += num + "\r\n";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new LotteryEntities())
                {
                    Shop_LuckDrawInfo info = new Shop_LuckDrawInfo();
                    info.CreateTime = DateTime.Now;
                    info.EndTime = DateTime.Now.AddDays(30);
                    info.LuckSummery = "测试活动";
                    info.LuckTitle = "测试";
                    info.MaxTry = 3;
                    info.StartTime = DateTime.Now;
                    info.State = 1;
                    info.UsedScore = 10;
                    db.Insert<Shop_LuckDrawInfo>(info);
                    this.textBox1.Text = info.ID.ToString();
                    this.richTextBox1.Text = JsonConvert.SerializeObject(db.Shop_LuckDrawInfo.ToList(), Formatting.Indented);
                }
            }
            catch (Exception ex)
            {
                this.textBox1.Text = ex.Message;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int activityID = Convert.ToInt32(this.textBox1.Text);
                using (var db = new LotteryEntities())
                {
                    for (var i = 0; i < 5; i++)
                    {
                        decimal rate = i < 4 ? 0.001m * (i+1) : 0.99m;
                        Shop_LuckAward info = new Shop_LuckAward()
                        {
                            LuckRate = rate,
                            isEmpty = i < 4 ? 0 : 1,
                            ProductCount = i < 4 ? (i+1) * 10 : int.MaxValue,
                            StockBalance = i < 4 ? (i +1)* 10 : int.MaxValue,
                            ProductID = i,
                            AwardPicture = "",
                            ProductName = "奖品"+(i+1),
                            LuckID = activityID,
                            CateCode = "",
                            CreateTime = DateTime.Now,
                            UpdateTimeStamp = new byte[0]
                        };
                        db.Insert<Shop_LuckAward>(info);
                    }
                    this.richTextBox1.Text = JsonConvert.SerializeObject(db.Shop_LuckAward.ToList(), Formatting.Indented);
                }
            }
            catch (Exception ex)
            {
                this.textBox1.Text = ex.Message;
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                string[] strs = this.textBox1.Text.Split('|');
                int activityID = Convert.ToInt32(strs[0]);
                int awardID = Convert.ToInt32(strs[1]);
                using (var db = new LotteryEntities())
                {
                    for (var i = 0; i < 2; i++)
                    {
                        Shop_LuckAwardStage stag = new Shop_LuckAwardStage()
                        {
                            LuckID = activityID,
                            CreateTime = DateTime.Now,
                            AwardID = awardID,
                            StageCount =awardID* 5 * (i+1),
                            StageTime = DateTime.Now.AddDays(5 * i + 5)
                        };
                        db.Insert<Shop_LuckAwardStage>(stag);
                    }
                    this.richTextBox1.Text = JsonConvert.SerializeObject(db.Shop_LuckAwardStage.ToList(), Formatting.Indented);
                }
            }
            catch (Exception ex)
            {
                this.textBox1.Text = ex.Message;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = JsonConvert.SerializeObject(lotteryCenter.GetCurrentRate(1), Formatting.Indented);
            for (var i = 0; i < 1000; i++)
            {
                Lottery.lotteryCenter.Lottery(3, 1);
            }

            this.richTextBox1.Text += "中率：\n";
            this.richTextBox1.Text += JsonConvert.SerializeObject(new
            {
                奖品1 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 1).Count() * 100 / Lottery.lotteryCenter.HisRecord(1).Count(),
                奖品2 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 2).Count() * 100 / Lottery.lotteryCenter.HisRecord(1).Count(),
                奖品3 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 3).Count() * 100 / Lottery.lotteryCenter.HisRecord(1).Count(),
                奖品4 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 4).Count() * 100 / Lottery.lotteryCenter.HisRecord(1).Count(),
                空奖5 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 5).Count() * 100 / Lottery.lotteryCenter.HisRecord(1).Count()
            }, Formatting.Indented);

            this.richTextBox1.Text += "中数：\n";

            this.richTextBox1.Text += JsonConvert.SerializeObject(new
            {
                奖品1 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 1).Count(),
                奖品2 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 2).Count(),
                奖品3 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 3).Count(),
                奖品4 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 4).Count(),
                空奖5 = Lottery.lotteryCenter.HisRecord(1).Where(p => p.Id == 5).Count()
            }, Formatting.Indented);


            this.richTextBox1.Text += JsonConvert.SerializeObject(Lottery.lotteryCenter.PrizeList(1), Formatting.Indented);
            if (!Lottery.lotteryCenter.HasPrize(1))
                this.richTextBox1.Text += "奖品被抽空了！";

            this.dataGridView1.DataSource = Lottery.lotteryCenter.Paramlist(1);
            writeLog();
        }
        private void writeLog()
        {
            log.Log.Info(this.richTextBox1.Text);
            log.Log.Info(JsonConvert.SerializeObject(Lottery.lotteryCenter.HisRecord(1), Formatting.Indented));
            log.Log.Info(JsonConvert.SerializeObject(this.dataGridView1.DataSource, Formatting.Indented));
            log.Log.Info(Lottery.lotteryCenter.PrizeRecord(1));
        }
        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Lottery.lotteryCenter.Create(1, new Action<Exception>(err =>
            {
                log.Log.Error("抽奖实例化失败！", err);
            }), new Action<Exception>(err =>
            {
                //运行中失败代理处理函数
                log.Log.Error("运行中失败！", err);
            })))
            {
                MessageBox.Show("ok!");
            }
            else {
                MessageBox.Show("error!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
           this.richTextBox1.Text=JsonConvert.SerializeObject(lotteryCenter.GetRateList(1), Formatting.Indented);
        }
    }
}
