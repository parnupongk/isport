<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="isport_sub._Default" %>

<%@ Import Namespace="System.Web.Services" %>

<script runat="server">
    [WebMethod]
    public static string GetRegions(int areaId)
    {
        return new isport_sub.Post.push().SendGet("http://wap.isport.co.th/isportws/isportsub.aspx");
        //return "44444444444";
    }
</script>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>jQuery and page methods</title>
    <script src="jquery-1.11.2.min.js" 
  type="text/javascript"></script> 
</head>
<body>
<div></div>
    <script type="text/javascript">

//        $.get("http://localhost/isportws/isportsub.aspx", function(data, status) {
//            alert("Data: " + data + "\nStatus: " + status);
//        });
        
//        $.getJSON("http://localhost/isportws/isportsub.aspx", function(result) {
//            $.each(result, function(i, field) {
//                $("div").append(field + " ");
//            });
//       
//       
//        });
        $(function() {
            var areaId = 42;
            $.ajax({
                type: "POST",
                url: "Default.aspx/GetRegions",
                data: "{areaId:" + areaId + "}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    alert(data.d);
                }
            });
        });

//        $(document).ready(function() {
//            $("button").click(function() {
//            $.get("http://localhost/isportws/isportsub.aspx", function(data, status) {
//                    alert("Data: " + data + "\nStatus: " + status);
//                });
//            });
//        });
    </script>

    <button>Send an HTTP GET request to a page and get the result back</button>
</body>
</html>
