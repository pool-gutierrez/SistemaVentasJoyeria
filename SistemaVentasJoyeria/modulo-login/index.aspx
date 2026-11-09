<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SistemaVentasJoyeria.modulo_login.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" type="image/png" href="/imagenes/logo.png"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" type="text/css" href="/Content/css/estilo.css"/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.7.0/animate.min.css" />
    <title>Login</title> 
</head>
<body>
    <header class="encabe">
    <div class="wrapper">
    <nav>
    <a href="/">Inicio</a>
    </nav>
	    <div class="titulo" >	
		    JOYERIA ROJAS	
	    </div>
    </div>
    </header>

    <form id="form1" runat="server"  class="login">
        <div class="logo" >
			<img class="logoo" src="/imagenes/logo.png" />
			<h3 >Inicia sesion</h3>
		</div>	

		<div class="grupo_log">
			<label>Ususario</label><br/>
            <asp:TextBox ID="TextBox1" CssClass="log"  runat="server"  placeholder="Escribe tu usuario"></asp:TextBox>
			<!--<input id="s" type="text" class="log" name="usuario" placeholder="Escribe tu usuario" />-->
		</div>
		<div class="grupo_log">
			<label>Contraseña</label><br/>
            <asp:TextBox ID="TextBox2" TextMode="Password" CssClass="log" runat="server"  placeholder="Escribe tu contraseña"></asp:TextBox>
			<!--<input id="ss" type="password" class="log" name="contraseña" placeholder="Escribe tu contraseña" />-->
		</div>

		<!-- <a href="#" >Registrate</a><br><br> -->
        <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="-Rellene todos los campos" Font-Bold="True" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <br />
        <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="-Usuario y/o contraseña incorrecto" Font-Bold="True" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <br />
        <asp:Label ID="Label3" runat="server" ForeColor="Red" Visible="False" Font-Bold="True" Font-Names="Arial" Font-Size="Small">-Usted ya no puede ingresar </asp:Label>
        <br />
        <asp:Button ID="Button1" CssClass="btn_ingresar" runat="server"   Text="Ingresar" OnClick="Button1_Click"  />
		<!--<input class="btn_ingresar" type="button" value="Ingresar" onclick="" />-->
    </form>
</body>
</html>

