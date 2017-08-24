<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="isport.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style5 {
            width: 140pt;
            text-align: center;
        }
        .auto-style6 {
            width: 140pt;
            text-align: center;
            background-color: #241D4F;
        }
        .auto-style7 {
            text-align: center;
            background-color: #241D4F;
        }
        .auto-style9 {
            color: #FFFFFF;
            text-align: center;
            background-color: #241D4F;
        }
        .auto-style10 {
            line-height: 2;
            font-family: Tahoma;
            color: #FFFFFF;
            margin-left: 0px;
            margin-right: 0px;
            margin-top: 0px;
            margin-bottom: 10px;
        }
        .auto-style11 {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <a href="#" onclick="javascript:window.open('http://www.gtalbot.org/BrowserBugsSection/MSIE7Bugs/ClosingWindowsNotOpenedByJS.html', '_self', '');">opent opoup</a>
        <asp:FileUpload ID="file" runat="server" />
        <asp:TextBox runat="server" ID="txt"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Upload" />
    


    </div>
     <table id="datatable1" class="table table-striped table-hover">
                <thead>
                    <tr>
                        <th rowspan="4" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">วันที่</th>
                        <th colspan="2" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan="3">ช่องทางการรับเรื่อง</th>
                        <th colspan="16" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Status</th>
                    </tr>
                    <tr>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan="3">Ope</th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan="3">Pending </th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" colspan="14" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Close</th>
                    </tr>
                    <tr>
                        <th colspan="3" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan="2">Complete</th>
                        <th colspan="4" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Escalate </th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan="2">Follow up</th>
                        <th rowspan="2" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Transfer to สคบ</th>
                        <th rowspan="2" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Transfer to อย.</th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan="2">Open – ปิดมือถือ</th>
                        <th rowspan="2" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Open – ไม่รับสาย</th>
                        <th rowspan="2" style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">Open – หมายเลขยังไม่เปิดให้บริการ</th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\" rowspan="2">Open – ฝากข้อความ</th>
                    </tr>
                    <tr>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">โทรศัพท์ </th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">อีเมล์ </th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">ETDA</th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">ปท</th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">ปอท</th>
                        <th style=\"background-color:#366092;color:#ffffff;font-weight:bold;\">ThaiCERT</th>
                    </tr>
                </thead>
                <tbody>

                    @foreach (var item in Model.list_repcase)
                    {
                        <tr>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                    </tr>
                        <tr class="gradeX">
                            <td></td>
                            <td>@item.casCreatedOn.Value.ToString("yyyy-MM-dd HH:mm:ss")</td>
                            <td>@item.casIDName</td>
                            <td>@item.chnName</td>
                            <id>@item.ctaNote</id>
                            <td>@item.ctaFullName</td>
                            <td>@item.ctaEmail</td>
                            <td>@item.ctaNumber</td>
                            <td>@item.casLevel2</td>
                            <td>@item.casLevel3</td>
                            <td>@item.casLevel6</td>
                            <td>@item.casreferenceDetail</td>
                            <td>&nbsp;</td>
                            <th>@item.casAttachFile</th>
                            <td>@item.casdetail</td>
                            <td>@item.casSolution</td>
                            <td>@item.casFollowDesc</td>
                            <td>@item.cssName</td>
                            <td>@item.casstatusReason</td>
                            <td></td>
                        </tr>

                    }
                </tbody>
            </table>

    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <p>
        &nbsp;</p>
    <table style="width: 677px">
        <tbody style="box-sizing: border-box;">
        <colgroup style="box-sizing: border-box;">
            <col style="box-sizing: border-box; width: 114pt;" />
            <col style="box-sizing: border-box; " />
            <col style="box-sizing: border-box; width: 127.5pt;" />
        </colgroup>
        <tbody style="box-sizing: border-box;">
            <tr style="box-sizing: border-box;">
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style9">
                    <p style="box-sizing: border-box; " class="auto-style10">
                        ขนาด/Size</p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style7">
                    <p style="box-sizing: border-box; " class="auto-style10">
                        รอบอก/Bust (นิ้ว/Inc.)</p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style6">
                    <p style="box-sizing: border-box; " class="auto-style10">
                        ความยาว/Length (นิ้ว/Inc.)</p>
                </td>
            </tr>
            <tr style="box-sizing: border-box;">
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style11">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
                        encoding</p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style11">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
        <asp:TextBox runat="server" ID="txtEncoding"></asp:TextBox>
                    </p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style5">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
                        &nbsp;</p>
                </td>
            </tr>
            <tr style="box-sizing: border-box;">
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style11">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
                        decoding</p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style11">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
        <asp:TextBox runat="server" ID="txtDecoding"></asp:TextBox>
                    </p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style5">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
                        &nbsp;</p>
                </td>
            </tr>
            <tr style="box-sizing: border-box;">
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style11">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
                        <span style="box-sizing: border-box; font-family: Tahoma;">L</span></p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style11">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
                        <span style="box-sizing: border-box; font-family: Tahoma;">41</span></p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style5">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
                        <span style="box-sizing: border-box; font-family: Tahoma;">29&nbsp;&nbsp; &nbsp;</span></p>
                </td>
            </tr>
            <tr style="box-sizing: border-box;">
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style11">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
                        <span style="box-sizing: border-box; font-family: Tahoma;">XL</span></p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style11">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;">
                        <span style="box-sizing: border-box; font-family: Tahoma;">43</span></p>
                </td>
                <td style="box-sizing: border-box; padding: 0px;" class="auto-style5">
                    <p style="box-sizing: border-box; margin: 0px 0px 10px; line-height: 2;" class="auto-style11">
                        <span style="box-sizing: border-box; font-family: Tahoma; line-height: 2; background-color: transparent;">30 &nbsp;<span class="Apple-converted-space">&nbsp;</span></span></p>
                </td>
            </tr>
        </tbody>
    </table>

    </form>
     
</body>
</html>
