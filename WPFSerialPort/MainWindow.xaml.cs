using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;  //시리얼통신을 위해 추가해줘야 함

namespace WPFSerialPort
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        SerialPort serialPort1 = new SerialPort();
        private void connection_Click(object sender, RoutedEventArgs e)
        {
            string[] ttt = SerialPort.GetPortNames(); //연결 가능한 시리얼포트 이름을 콤보박스에 가져오기

            if (!serialPort1.IsOpen)  //시리얼포트가 열려 있지 않으면
            {
                serialPort1.PortName = "COM3";
                serialPort1.BaudRate = 9600;  //보레이트 변경이 필요하면 숫자 변경하기
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Parity = Parity.None;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived); //이것이 꼭 필요하다

                serialPort1.Open();  //시리얼포트 열기

                label_status.Text = "포트가 열렸습니다.";
            }
            else  //시리얼포트가 열려 있으면
            {
                label_status.Text = "포트가 이미 열려 있습니다.";
            }

            //for (int i = 0; i < 10; i++)
            //{
            //    serialPort1.WriteLine("asdfasdfasfasdf");  //텍스트박스의 텍스트를 시리얼통신으로 송신
            //}
            serialPort1.Write(new byte[] { 0x10, 0x04, 0x04 }, 0, 3);
        }

        // 수신 이벤트가 발생하면 이 부분이 실행된다.
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                int ReceiveData = serialPort1.ReadByte();  //시리얼 버터에 수신된 데이타를 ReceiveData 읽어오기

                if (ReceiveData == 0x12)
                {
                    // 정상
                    int inReceive = 10;

                    richTextBox_received.Text = inReceive.ToString();
                }
            }));
        }


        private void deconnection_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort1.IsOpen)  //시리얼포트가 열려 있으면
            {
                serialPort1.Close();  //시리얼포트 닫기

                label_status.Text = "포트가 닫혔습니다.";
            }
            else  //시리얼포트가 닫혀 있으면
            {
                label_status.Text = "포트가 이미 닫혀 있습니다.";
            }
        }
    }
}
