<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineLoginAuth.aspx.cs" Inherits="LazyStock.Web.LineLoginAuth" %>

<html>
<head>
    <title></title>
    <meta charset="utf-8" />
    <script>
        function Auth(){ 
            var URL = 'https://access.line.me/oauth2/v2.1/authorize?';
            URL += 'response_type=code';
            URL += '&client_id=<%=Common.Tools.Setting.AppSettings("LineAuthId")%>';  //self client_id
            URL += '&redirect_uri=<%=Common.Tools.Setting.AppSettings("LineAuthCallBackUrl") %>';
            URL += '&scope=openid%20profile';
            URL += '&state=abcde';
            
            window.location.href = URL;
        }

        Auth();
    </script>
</head>
<body>
</body>
</html>