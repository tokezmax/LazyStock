<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LineLoginAuth.aspx.cs" Inherits="LazyStock.Web.LineLoginAuth" %>

<html>
<head>
    <title></title>
    <meta charset="utf-8" />
    <script>
        function Auth(){ 
            var URL = 'https://access.line.me/oauth2/v2.1/authorize?';
            URL += 'response_type=code';
            URL += '&client_id=1593644840';  //請換成你自己的 client_id
            URL += '&redirect_uri=http://localhost:2458/Member/CallbackFromLine'; //請換成你自己的 callback url
            URL += '&scope=openid%20profile';
            URL += '&state=abcde';
            //導引到LineLogin
            //window.location.href = URL;
            document.getElementById('search_iframe').src = URL;
            //document.getElementById['search_iframe'].src = 'http://localhost:2458/default.aspx'

            window.open(URL, '新視窗的名稱', config='height=1024px,width=1024px,toolbar=no,scrollbars=no,resizable=no,location=no,status=no,menubar=no');
        }
    </script>
</head>
<body>
    <iframe id="search_iframe" src="" style="width:1024px;height:1024px">


    </iframe>
    <button onclick="Auth();">點選這裡連結到LineLogin</button>
</body>
</html>

