using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Net;
using System.Windows;

namespace Invasion
{
    class InputServer //got help from http://www.developerfusion.com/article/3997/socket-programming-in-c-part-2/4/
    {
        public static Socket m_socListener;
        public static byte[] m_DataBuffer = new byte[100];
        public static IAsyncResult m_asynResult;
        public static AsyncCallback pfnCallBack;
       // public static Socket m_socClient;
        public static Socket m_socWorker;


        
        public static void StartListening()
        {
            try{
                //create the listening socket...
                m_socListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, 8221);
                //bind to local IP Address...
                m_socListener.Bind(ipLocal);
                //start listening...
                m_socListener.Listen(4);
                // create the call back for any client connections...
                m_socListener.BeginAccept(new AsyncCallback(OnClientConnect), null);

                Console.WriteLine("SERVER STARTED");
                //cmdListen.Enabled = false;
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public static void OnClientConnect(IAsyncResult asyn)
        {
            try
            {
                Console.WriteLine("In onclientconnect");
                m_socWorker = m_socListener.EndAccept(asyn);
                WaitForData();
                
                
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\n OnClientConnection: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
        }

        public static void WaitForData()
        {
            Console.WriteLine("In waitfordata");
            if (pfnCallBack == null)
                pfnCallBack = new AsyncCallback(OnDataReceived);
            // now start to listen for any data...
            m_asynResult = m_socWorker.BeginReceive(m_DataBuffer, 0, m_DataBuffer.Length, SocketFlags.None, pfnCallBack, null);
        }
        public static void OnDataReceived(IAsyncResult asyn)
        {
            Console.WriteLine("In ondata received");
            //end receive...
            int iRx = 0;
            iRx = m_socWorker.EndReceive(asyn);
            char[] chars = new char[iRx + 1];
            System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
            int charLen = d.GetChars(m_DataBuffer, 0, iRx, chars, 0);
            System.String szData = new System.String(chars);
            Console.WriteLine(szData);
            m_socWorker.Send(m_DataBuffer);
            WaitForData();
        }
    
    }   

}
