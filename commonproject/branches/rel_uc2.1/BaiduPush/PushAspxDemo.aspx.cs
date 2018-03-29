/* 
    Copyright (c) 2011 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://go.microsoft.com/fwlink/?LinkID=219604 
  
*/
using System;
using System.Net;
using System.IO;
using System.Text;

namespace BaiduPush
{
    public partial class PushAspxDemo : System.Web.UI.Page
    {

        protected void BtnSend_Click(object sender, EventArgs e)
        {
            int issuccess=0;
            string sucome = "{\"pushtype\": \"notification\",\"data\": { \"key\": \"value\"} }";
            sucome = "{\"pushtype\": \"passmsg\",\"data\": { \"key\": \"value\"} }";
            string result = PushSend.Send("≤‚ ‘Õ∆Ãÿ", "ƒ⁄»›", "4498473584697613830", "688520328761376249", sucome, "yfk", ref issuccess);
            TextBoxResponse.Text = result;

           // return;

            try
            {
                string sk = TBSK.Text;
                string ak = TBAK.Text;
                BaiduPush Bpush = new BaiduPush("POST", sk);
                String apiKey = ak;
                String messages = "";
                String method = "push_msg";
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                uint device_type = 3;
                uint unixTime = (uint)ts.TotalSeconds;

                uint message_type;
                string messageksy = "xxxxxx";
                if (RbMessage.Checked)
                {
                    message_type = 0;
                    messages = TBMessage.Text;
                }
                else
                {
                    message_type = 1;

                    if (RBIOSPRO.Checked == true || RBIOSDEV.Checked == true)
                    {
                        device_type = 4;
                        IOSNotification notification = new IOSNotification();
                        notification.title = TBTitle.Text;
                        notification.description = TBDescription.Text;
                        // notification.url = "http://www.cmb.yungujia.com";
                        notification.user_confirm = 1;
                        notification.open_type = 2;
                        notification.notification_basic_style = 4;
                        notification.notification_builder_id = 0;
                        notification.pkg_content = "http://www.baidu.com";
                        messages = notification.getJsonString();

                    }
                    else
                    {
                        BaiduPushNotification notification = new BaiduPushNotification();
                        notification.title = TBTitle.Text;
                        notification.description = TBDescription.Text;

                        //notification.url = "http:cmb.yungujia.com";
                        notification.user_confirm = 1;
                        notification.open_type = 2;
                        notification.notification_basic_style = 4;
                        notification.notification_builder_id = 0;
                        notification.pkg_content = "entrust.html";
                        notification.custom_content = "{entrustid:1}";
                        messages = notification.getJsonString();

                    }
                }


                PushOptions pOpts;
                if (RBUnicast.Checked)
                {
                    pOpts = new PushOptions(method, apiKey, TBUserId.Text, TBChannelID.Text, device_type, messages, messageksy, unixTime);
                }
                else if (RBMulticast.Checked)
                {
                    pOpts = new PushOptions(method, apiKey, TBTag.Text, device_type, messages, messageksy, unixTime);
                }
                else
                {
                    pOpts = new PushOptions(method, apiKey, device_type, messages, messageksy, unixTime);
                }

                pOpts.message_type = message_type;
                if (RBIOSPRO.Checked == true)
                {
                    pOpts.deploy_status = 2;
                }
                else if (RBIOSDEV.Checked == true)
                {
                    pOpts.deploy_status = 1;
                }

                string response = Bpush.PushMessage(pOpts);

                TextBoxResponse.Text = response;

            }
            catch (Exception ex)
            {
                TextBoxResponse.Text = "Exception caught sending update: " + ex.ToString();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string sk = TBSK.Text;
                string ak = TBAK.Text;
                BaiduPush Bpush = new BaiduPush("POST", sk);
                String apiKey = ak;
                String messages = "";
                String method = "fetch_tag";
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                uint device_type = 3;
                uint unixTime = (uint)ts.TotalSeconds;
                string messageksy = "xxxxxx";

                PushOptions pOpts = new PushOptions(method, apiKey, TBUserId.Text, TBChannelID.Text, device_type, messages, messageksy, unixTime);

                string response = Bpush.PushMessage(pOpts);

                TextBoxResponse.Text = response;

            }
            catch (Exception ex)
            {
                TextBoxResponse.Text = "Exception caught sending update: " + ex.ToString();
            }
        }
    }
}
