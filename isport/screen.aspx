<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="screen.aspx.cs" Inherits="isport.screen" %>
<script type="text/javascript">
// <![CDATA[
	if (window.location.href.search(/_height=\d+&_width=\d+/) == -1) {
		// add, or append querystring
		var punct = window.location.href.search(/\?/) > 0 ? "&" : "?";
		// call back and tell the server what the window parameters are
		window.location.href += punct +
			"_height=" + document.documentElement.clientHeight +
			"&_width=" + document.documentElement.clientWidth;
	}
//]]>
</script>