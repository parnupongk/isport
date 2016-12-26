<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="isport_sub.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Subscribe</title>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <meta name="description" content=""/>
    <meta name="author" content=""/>
    <link rel="shortcut icon" href="../images/logo.ico" />
    <link href="dist/css/bootstrap.css" rel="stylesheet"/>
    <link href="dist/css/customCSS.css" rel="Stylesheet"/>
    <link href="navbar-fixed-top.css" rel="stylesheet">
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
      <script src="html5shiv.js"></script>
      <script src="respond.min.js"></script>
    <![endif]-->
    
</head>

<body>
    <div id="divMenuHeader" runat="server" class="navbar navbar-default navbar-fixed-top" role="navigation" >
      <div class="container">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="#"><h2>สมัครบริการ</h2></a>
        </div>
        <div class="navbar-collapse collapse">
          <ul class="nav navbar-nav">
            <li class="active"><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamdaraclip_sub">คลิปลับดารา</a></li>
            <!-- li><a href="http://wap.isport.co.th/isportui/indexh.aspx?p=siamlucky">เลขเด็ดเศรษฐี</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexh.aspx?p=siamhoro">เช็คดวงวันนี้</a></li -->
          </ul>
          <ul class="nav navbar-nav navbar-right">
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=siamvarity">ประสบการณ์รักพริตตี้</a></li>
            <li><a href="http://wap.isport.co.th/isportui/indexl.aspx?p=bb">Isport</a></li>
          </ul>
        </div><!--/.nav-collapse -->
      </div>
    </div>   
    
    
    <form id="frmMain" runat="server">
    
    <div class="row featurette">
        <div class="page-header">
                   <asp:Label ID="lblTitle" runat="server"></asp:Label>
        </div>
        
        <section  id="reqOTP">
		                 <!--CONTACT FORM-->
		                 <div class="contact">
                          <div class="container">
                            <div class="row">
			                  <h2 class="text-center">สมัครบริการ</h2>  
                              <div class="col-md-7 col-sm-6 col-xs-12">
                              
					                  <div class="form-group ">
						                <label for="exampleInputName">กรุณากรอกหมายเลขโทรศัพท์มือถือที่ต้องการรับบริการ</label>
						                <input type="text" class="form-control input-lg" id="inMobile" maxlength="10"   placeholder="เบอร์โทรศัพท์" required>
					                  </div>
					                  <div class="form-group">
						                <label for="exampleInputEmail1" >เครือค่าย</label>
						                <select id="ddlOPT" class="form-control input-lg" >
						                <option value="99">เครือค่าย</option>
                                            <option value="01">AIS</option>
                                            <option value="02">Dtac</option>
                                            <option value="03">TrueMove</option>
                                            <option value="04">TrueMoveH</option>
                                            <option value="05">3Gx</option>
                                            </select>
					                  </div>
                						
					                  <div class=""><a id="btnGetOTP" class="btn btn-success btn-lg" href="#" onclick="javascript:GetOTP();return false;">ยืนยัน</a></div>
				                 
                              </div>
                              
			                  <!--/CONTACT FORM-->
                			  
                              <!--ADMNISTRATION PROPILE-->			  
                              <div class="col-md-4 col-sm-6 col-xs-12 col-md-offset-1 col-sm-offset-1 ">
			                   <ul class="list-unstyled text-center">
			                   <li><h3>Adminstrator</h3></li><br>
			                    <li><img src="images/tm.jpg" class="contact-img" alt="img02"></li><br>
				                <li><h4>Daniel,CEO</h4></li>
				                <li><i class="fa fa-mobile fa-2x"> </i>  505. 666 777</li>
				                <li><i class="fa fa-envelope-o fa-2x"> </i><a href="#"> imshaifuddin92@gmail.com </a></li><br>
				                <li>
				                <a href="#">
				                 <span class="fa-stack fa-2x">
				                  <i class="fa fa-circle fa-stack-2x"></i>
				                  <i class="fa fa-facebook fa-stack-1x con-ft"></i>
				                 </span></a>
                                <a href="#">
				                <span class="fa-stack fa-2x">
				                  <i class="fa fa-circle fa-stack-2x"></i>
				                  <i class="fa fa-twitter fa-stack-1x con-ft"></i>
				                 </span></a>
				                 <a href="#">
                                 <span class="fa-stack fa-2x">
				                  <i class="fa fa-circle fa-stack-2x"></i>
				                  <i class="fa fa-google-plus fa-stack-1x con-ft"></i>
				                 </span></a>				 
				                </li>
			                   </ul>			  
                              </div>
			                  <!--/ADMNISTRATION PROPILE-->
                           </div>
                          </div>
		                 </div> 
		 <!--/CONTACT-->
		         </section>
        
        <!--SERVICES SECTION-->		   
       <section id="subOTP">
	     <div class="service"> 
          <div class="container">
            <div class="row">
			   <h2 class="text-center">Provided Services</h2>  
			          <div class="col-md-7 col-sm-6 col-xs-12">

					                  <div class="form-group ">
						                <label for="exampleInputName">กรุณากรอกรหัส ที่ได้รับทาง SMS</label>
						                <input type="text" class="form-control input-lg" id="inOTP" maxlength="4" placeholder="รหัส OTP">
					                  </div>
                						
					                  <div class="">
					                  <a  class="btn btn-success btn-lg" href="#" onclick="javascript:SubOTP();return false;" id="btnSubmitOTP">
					                   ยืนยัน</a></div>
       
                        </div>
                        <!--ADMNISTRATION PROPILE-->			  
                              <div class="col-md-4 col-sm-6 col-xs-12 col-md-offset-1 col-sm-offset-1 ">
			                   <ul class="list-unstyled text-center">
			                   <li><h3>Adminstrator</h3></li><br>
			                    <li><img src="images/tm.jpg" class="contact-img" alt="img02"></li><br>
				                <li><h4>Daniel,CEO</h4></li>
				                <li><i class="fa fa-mobile fa-2x"> </i>  505. 666 777</li>
				                <li><i class="fa fa-envelope-o fa-2x"> </i><a href="#"> imshaifuddin92@gmail.com </a></li><br>
				                <li>
				                <a href="#">
				                 <span class="fa-stack fa-2x">
				                  <i class="fa fa-circle fa-stack-2x"></i>
				                  <i class="fa fa-facebook fa-stack-1x con-ft"></i>
				                 </span></a>
                                <a href="#">
				                <span class="fa-stack fa-2x">
				                  <i class="fa fa-circle fa-stack-2x"></i>
				                  <i class="fa fa-twitter fa-stack-1x con-ft"></i>
				                 </span></a>
				                 <a href="#">
                                 <span class="fa-stack fa-2x">
				                  <i class="fa fa-circle fa-stack-2x"></i>
				                  <i class="fa fa-google-plus fa-stack-1x con-ft"></i>
				                 </span></a>				 
				                </li>
			                   </ul>			  
                              </div>
			                  <!--/ADMNISTRATION PROPILE-->
			 </div>
            </div>  			 
            
            
            		 <div class="features-bg">  
		              <div class="container">
		                 <div class="row">
			                    <div class="col-md-2 col-sm-4 col-xs-12">
        				            <a href=""><h2>Extra Features</h2></a>
        			            </div>  
		                        <div class="col-md-2 col-sm-4 col-xs-12">
        				            <a href="#"><img src="images/amazon.png" class="features pull-right img-responsive" alt="Features-Title"></a>
        			            </div>
					             <div class="col-md-2 col-sm-4 col-xs-12">
        				            <a href="#"><img src="images/wp.png" class="features pull-right img-responsive" alt="jquery"></a>
        			            </div>
					             <div class="col-md-2 col-sm-4 col-xs-12">
        				            <a href="#"><img src="images/windows.png" class="features pull-right img-responsive" alt="jquery"></a>
        			            </div>
					             <div class="col-md-2 col-sm-4 col-xs-12">
        				            <a href="#"><img src="images/android.png" class="features pull-right  img-responsive" alt="jquery"></a>
        			            </div>
					            <div class="col-md-2 col-sm-4 col-xs-12">
        				            <a href="#"><img src="images/apple.png" class="features pull-right img-responsive" alt="Img Title"></a>
        			            </div>
		                    </div>
		                  </div> 
		            </div>
            
          </div>
         <!--/SERVICES-->         
         
         		 <!--EXTRA FEATURES-->

		  <!--/EXTRA FEATURES-->
         
		</section>
		<!--/SERVICES SECTION--> 
		
		
		<!--THANK SECTION-->		   
       <section id="thankSEc">
	     <div class="services"> 
          <div class="container">
            <div class="row">
			   <h2 class="text-center">Thank forSubscribe</h2>  
			          <div class="col-md-12 col-sm-12 col-xs-12">

					                  <ul class="list-unstyled text-center">
			                   <li><h3>ขอบคุณทีใช้บริการ </h3></li><br>
				                <li><h4>กรุณารอรับ sms ยืนยันการสมัครบริการ หรือโทร</h4></li>
				                <li><i class="fa fa-mobile fa-2x"> </i>  02-5026767</li>
				                <li><i class="fa fa-envelope-o fa-2x"> </i><a href="#"> contact@isport.co.th </a></li><br>
				                <li><asp:Label ID="lblbanner" runat="server"></asp:Label></li><br>
				                <li>
				                <a href="#">
				                 <span class="fa-stack fa-2x">
				                  <i class="fa fa-circle fa-stack-2x"></i>
				                  <i class="fa fa-facebook fa-stack-1x con-ft"></i>
				                 </span></a>
                                <a href="#">
				                <span class="fa-stack fa-2x">
				                  <i class="fa fa-circle fa-stack-2x"></i>
				                  <i class="fa fa-twitter fa-stack-1x con-ft"></i>
				                 </span></a>
				                 <a href="#">
                                 <span class="fa-stack fa-2x">
				                  <i class="fa fa-circle fa-stack-2x"></i>
				                  <i class="fa fa-google-plus fa-stack-1x con-ft"></i>
				                 </span></a>				 
				                </li>
			                   </ul>	
       
                        </div>
			 </div>
            </div>  			 
          </div>
         <!--/SERVICES-->         
		</section>
		<!--/THANK SECTION--> 
		
        
        <div class="row">
        <div class="col-md-8">
                
        </div>

        
        <div class="col-md-4">
            <div class="panel panel-success">
              <div class="panel-body">
                
              </div>
            </div>

        </div>
        
        </div>
    </div>

      <hr class="featurette-divider">
      
        <asp:Label ID="lblFooter" runat="server"></asp:Label>

    </form>
    

    
    
            <footer>

      
        <p class="pull-right"><a href="#">Back to top</a></p>
        <p>&copy; 2013 Company, Isport. &middot;</p>
     </footer>
     


    <script src="assets/js/jquery.js"></script>
    <script src="dist/js/bootstrap.min.js"></script>
    <script src="assets/js/holder.js"></script>
    
    <script type="text/javascript" 
src="jquery-1.11.2.min.js"></script>

    
    <script type="text/javascript">
        var otp = "";
        function GetOTP() {


            if ($("#inMobile").val().length != "10") {
                alert("กรุณาระบุ เบอร์โทรศัพท์");
                $("#inMobile").focus();
            }
            else if ($("#ddlOPT").val() == "99") {
                alert("กรุณาระบุ เครือค่าย");
                $("#ddlOPT").focus();
            }
             
            else {
                $('html, body').animate({
                    scrollTop: $("#subOTP").offset().top
                }, 2000);


                $.ajax({
                    type: "POST",
                    url: "index.aspx/GetOTP",
                    data: "{msisdn:" + $("#inMobile").val() + " , opt: " + $("#ddlOPT").val() + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(response) {
                        // alert(response.toString());
                        otp = response.d;
                        $("#inOTP").focus();
                    }
                });

                
            }

            return false;
        }

        function SubOTP() {
            if ($("#inOTP").val() == "") {
                alert("กรุณาระบุ รหัสที่ได้รับจาก SMS");
            } else if ($("#inOTP").val() != otp) {
                alert("ท่านกรอก รหัสไม่ถูกต้องกรุณากรอกใหม่");
                alert(otp);
                alert($("#inOTP").val());
                $("#inOTP").val() = "";
            } else if ($("#inOTP").val() == otp) {
            
                $('html, body').animate({
                    scrollTop: $("#thankSEc").offset().top
                }, 2000);

                var pssvId = GetParameterValues("pssvid");

                $.ajax({
                    type: "POST",
                    url: "index.aspx/SubmitOTP",
                    data: "{msisdn:'" + $("#inMobile").val() + "' , opt: '" + $("#ddlOPT").val() + "', pssvid:'" + pssvId + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function(response) {
                        alert(response.d);
                    }
                });
            }   // end else if
                return false;
        }


        
        
        function GetParameterValues(param) {
            var url = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for (var i = 0; i < url.length; i++) {
                var urlparam = url[i].split('=');
                if (urlparam[0] == param) {
                    return urlparam[1];
                }
            }
        }
        
//        $(document).ready(function() {
//            $("#btnGetOTP").click(function() {
//                //$(this).animate(function(){
//                //alert($("#inMobile").val());

////                $.ajax({
////                    type: "POST",
////                    url: "index.aspx/GetOTP",
////                    data: "{msisdn:" + $("#inMobile").val() + " , opt: " + $("#ddlOPT").val() + "}",
////                    contentType: "application/json; charset=utf-8",
////                    dataType: "json",
////                    success: function(response) {
////                        alert(response.toString());
////                    }
////                });


//                $('html, body').animate({
//                    scrollTop: $("#subOTP").offset().top
//                }, 2000);

//            });

//        });


</script>
    
    		
	    
	    
</body>
</html>

