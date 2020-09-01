using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginNormalParam
{
    public static LoginNormalReq parseLoginResult(string str)
    {
        object jsonParsed = MiniJSON.Json.Deserialize(str);
        if (jsonParsed != null)
        {
            Dictionary<string, object> jsonMap = jsonParsed as Dictionary<string, object>;
            LoginNormalReq data = new LoginNormalReq();

            if (jsonMap.ContainsKey("loginName"))
            {
                data.loginName = jsonMap["loginName"].ToString();
            }

            if (jsonMap.ContainsKey("sign"))
            {
                data.sign = jsonMap["sign"].ToString();
            }

            if (jsonMap.ContainsKey("time"))
            {
                data.time =long.Parse(jsonMap["time"].ToString());
            }
            if(jsonMap.ContainsKey("user_id"))
            {
                data.user_id = jsonMap["user_id"].ToString();
            }
            if (jsonMap.ContainsKey("token"))
            {
                data.token = jsonMap["token"].ToString();
            }

            if (jsonMap.ContainsKey("ext"))
            {
                data.ext = jsonMap["ext"].ToString();
            }
            return data;
        }
        return null;
    }
}

public class LoginNormalReq
{
    public string loginName { get; set; }
    public string sign { get; set; }
    public long time { get; set; }

    public string user_id { get; set; }

    public string token { get; set; }

    public string ext { get; set; }
}
