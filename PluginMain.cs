using System.Collections.Generic;
using System.Net;
using System.Text;
using FNF.BouyomiChanApp;
using FNF.Utility;
using FNF.XmlSerializerSetting;
using Hapo31.kusidango.Entity;
using Hapo31.kusidango.Service;
using Hapo31.kusidango.Util;

namespace Hapo31.kusidango
{
    public class PluginMain : IPlugin
    {
        string IPlugin.Name => Properties.Resources.Name;

        string IPlugin.Version => Properties.Resources.Version;

        string IPlugin.Caption => Properties.Resources.Caption;

        ISettingFormData IPlugin.SettingFormData => null;

        private HttpServer http;

        void IPlugin.Begin()
        {
            http = new HttpServer(51400);
            http.OnReceiveRequest += OnReceiveRequestHandler;
            http.Listen();
        }

        void IPlugin.End()
        {
            http.Close();
        }

        private void OnReceiveRequestHandler(HttpListenerRequest req, HttpListenerResponse res)
        {
            var query = req.RawUrl.QueryParse();
            var talkData = createDataFromQuery(query);
            Pub.AddTalkTask(talkData.Message, talkData.Speed, talkData.Tone, talkData.Volume, talkData.Voice);
        }


        private TalkData createDataFromQuery(Dictionary<string, string> query)
        {
            if (query.ContainsKey("message"))
            {
                return new TalkData
                {
                    Message = System.Web.HttpUtility.UrlDecode(query["message"]),
                    Speed = TryOrDefault(query, "speed", -1),
                    Tone = TryOrDefault(query, "tone", -1),
                    Volume = TryOrDefault(query, "volume", -1),
                    Voice = (VoiceType)TryOrDefault(query, "voice", (int)VoiceType.Default)
                };
            }

            return new TalkData();
        }

        private int TryOrDefault(Dictionary<string, string> query, string name, int defaultValue)
        {
            try
            {
                return int.Parse(query[name]);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
