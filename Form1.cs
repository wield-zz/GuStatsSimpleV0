using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public int won = 0;
        public int total_won = 0;
        public int lost = 0;
        public int total_lost = 0;
        public string player_id = "";

        public int start_time = 1640347200;
        public int end_time = 1640606400;
        public Form1()
        {
            InitializeComponent();
        }

        public async Task getXAsync()
        {
            won = 0;
            player_id = textBox1.Text;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.godsunchained.com/v0/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync("match?player_won="+player_id+ "&page=1&perPage=500&start_time=" + start_time + "-" + end_time );
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Read");
                    var stream = await response.Content.ReadAsStreamAsync();

                    using (var streamReader = new StreamReader(stream))
                    {
                        using (var jsonTextReader = new JsonTextReader(streamReader))
                        {
                            var jsonSerializer = new JsonSerializer();
                            var datas = jsonSerializer.Deserialize<Stats>(jsonTextReader);
                            //Console.WriteLine(datas.total);
                            total_won = datas.total;
                            foreach (Records d in datas.records)
                            {
                                
                                if(d.total_rounds > 5)
                                {
                                    won++;
                                }
                            }
                        }
                    }
                  
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
        }
        public async Task getYAsync()
        {

            
            using (var client = new HttpClient())
            {
                lost = 0;
                client.BaseAddress = new Uri("https://api.godsunchained.com/v0/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //GET Method
                HttpResponseMessage response = await client.GetAsync("match?player_lost=" + player_id + "&page=1&perPage=500&start_time=" + start_time + "-" + end_time);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Read");
                    var stream = await response.Content.ReadAsStreamAsync();

                    using (var streamReader = new StreamReader(stream))
                    {
                        using (var jsonTextReader = new JsonTextReader(streamReader))
                        {
                            var jsonSerializer = new JsonSerializer();
                            var datas = jsonSerializer.Deserialize<Stats>(jsonTextReader);
                            //Console.WriteLine(datas.total);
                            total_lost = datas.total;
                            foreach (Records d in datas.records)
                            {

                                if (d.total_rounds > 5)
                                {
                                    lost++;
                                }
                            }
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(getXAsync).Wait();
            Task.Run(getYAsync).Wait();
            int total = total_won + total_lost;
            int total_count = won + lost;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(start_time);
            DateTime dateTime = dateTimeOffset.DateTime;
            DateTimeOffset dateTimeOffset1 = DateTimeOffset.FromUnixTimeSeconds(end_time);
            DateTime dateTime1 = dateTimeOffset1.DateTime;
            label1.Text = "Total won matches: " + total_won + " | Total lost matches: " + total_lost +
                          "\r\nTotal games played: " + total +
                          "\r\n \r\nWon (More than 5 rounds):" + won + "\r\nLost (More than 5 rounds):" + lost +
                          "\r\nSum of counted games: " + total_count +
                          "\r\n \r\nStart time: " + dateTime + "\r\n End time:" + dateTime1 ;

        }
    }
}
