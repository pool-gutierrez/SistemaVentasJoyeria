<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="comprobanteset.aspx.cs" Inherits="SistemaVentasJoyeria.modulo_reportes.comprobanteset" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <meta name="description" content="Sistema de Ventas"/>
    <link rel="shortcut icon" type="image/png" href="/imagenes/logo.png"/>
    <title>Sistema de Ventas</title>
    <link href="/Content/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css"/>
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet"/>
    <link href="/Content/css/sb-admin-2.min.css" rel="stylesheet"/>
</head>
<body id="page-top">
    <div id="wrapper">
        <ul class="navbar-nav bg-gradient-primary sidebar sidebar-dark accordion" id="accordionSidebar">

            <!-- Menu Principal -->
            <a class="sidebar-brand d-flex align-items-center justify-content-center" href="/modulo-main/principalset.aspx">
                <div class="sidebar-brand-icon rotate-n-15">
                    <img src="../img/diamante.png" width="40"/>
                </div>
                <div class="sidebar-brand-text mx-3">Joyeria Rojas</div>
            </a>

            <hr class="sidebar-divider my-0">

            <li class="nav-item">
            <a class="nav-link" href="/modulo-main/principalset.aspx">
                <i class="fas fa-fw fa-warehouse"></i>
                <span>Principal</span></a>
            </li>

            <hr class="sidebar-divider">

            <div class="sidebar-heading">
            Negocio
            </div>

            <!-- Negocio -->
            <li class="nav-item">
            <a class="nav-link collapsed" href="#" data-toggle="collapse" data-target="#collapsePages" aria-expanded="true" aria-controls="collapsePages">
                <i class="fas fa-fw fa-cart-plus"></i>
                <span>Compras y Ventas</span>
            </a>
            <div id="collapsePages" class="collapse" aria-labelledby="headingPages" data-parent="#accordionSidebar">
                <div class="bg-white py-2 collapse-inner rounded">
                <h6 class="collapse-header">Almacén</h6>
                <a class="collapse-item" href="/modulo-producto/producto.aspx"><i class="fas fa-fw fa-gem"></i>&nbsp;&nbsp;&nbsp;Joyas</a>
                <a class="collapse-item" href="/modulo-categoria/categoria.aspx"><i class="fas fa-fw fa-indent"></i>&nbsp;&nbsp;&nbsp;Categorias</a>
                <div class="collapse-divider"></div>
                <h6 class="collapse-header">Compras</h6>
                <a class="collapse-item" href="/modulo-compra/nuevacompra.aspx"><i class="fas fa-fw fa-boxes"></i>&nbsp;&nbsp;&nbsp;Nueva Compra</a>
                <a class="collapse-item" href="/modulo-proveedor/proveedor.aspx"><i class="fas fa-fw fa-dolly"></i>&nbsp;&nbsp;&nbsp;Proveedores</a>
                <div class="collapse-divider"></div>
                <h6 class="collapse-header">Ventas</h6>
                <a class="collapse-item" href="/modulo-venta/nuevaventa.aspx"><i class="fas fa-fw fa-cash-register"></i>&nbsp;&nbsp;&nbsp;Nueva Venta</a>
                <a class="collapse-item" href="/modulo-cliente/cliente.aspx"><i class="fas fa-fw fa-users"></i>&nbsp;&nbsp;&nbsp;Clientes</a>
                </div>
            </div>
            </li>

            <hr class="sidebar-divider">

            <div class="sidebar-heading">
            Reportes
            </div>

            <li class="nav-item active">
            <a class="nav-link" href="/modulo-reportes/comprobanteset.aspx">
                <i class="fas fa-fw fa-file-alt"></i>
                <span>Consultar comprobantes</span></a>
            </li>

            <li class="nav-item">
            <a class="nav-link" href="/modulo-reportes/venta.aspx">
                <i class="fas fa-fw fa-file-alt"></i>
                <span>Consultar ventas</span></a>
            </li>

            <li class="nav-item">
            <a class="nav-link" href="/modulo-reportes/compra.aspx">
                <i class="fas fa-fw fa-file-alt"></i>
                <span>Consultar compras</span></a>
            </li>

            <li class="nav-item">
            <a class="nav-link" href="/modulo-reportes/producto.aspx">
                <i class="fas fa-fw fa-file-alt"></i>
                <span>Consultar productos</span></a>
            </li>

            <hr class="sidebar-divider d-none d-md-block">

            <!-- Reduce el menú principal -->
            <div class="text-center d-none d-md-inline">
            <button class="rounded-circle border-0" id="sidebarToggle"></button>
            </div>
        </ul>

        <!-- Barra superior -->
        <div id="content-wrapper" class="d-flex flex-column">
            <div id="content">
                <nav class="navbar navbar-expand navbar-light bg-white topbar mb-4 static-top shadow">

                    <button id="sidebarToggleTop" class="btn btn-link d-md-none rounded-circle mr-3">
                    <i class="fa fa-bars"></i>
                    </button>

                    <ul class="navbar-nav ml-auto">
                        <!-- Usuario -->
                        <li class="nav-item dropdown no-arrow">
                            <a class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <asp:label runat="server" ID="lblusuario" Text="Nombre de usuario" CssClass="mr-2 d-none d-lg-inline text-gray-600 small"></asp:label>
                                <img class="img-profile rounded-circle" src="https://static.vecteezy.com/system/resources/previews/000/550/731/non_2x/user-icon-vector.jpg">
                            </a>
                            <!-- Menú desplegable de usuario -->
                            <div class="dropdown-menu dropdown-menu-right shadow animated--grow-in" aria-labelledby="userDropdown">
                            <a class="dropdown-item" href="#">
                                <i class="fas fa-user fa-sm fa-fw mr-2 text-gray-400"></i>
                                <asp:label runat="server" ID="lblpersona" Text="Nombre de usuario"></asp:label>
                            </a>
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item" href="#" data-toggle="modal" data-target="#logoutModal">
                                <i class="fas fa-sign-out-alt fa-sm fa-fw mr-2 text-gray-400"></i>
                                Cerrar Sesión
                            </a>
                            </div>
                        </li>
                    </ul>
                </nav>

                <div class="container-fluid">
                    <!-- Aquí se ponen todos los formularios -->
                    <div class="card shadow mb-4 border-left-primary" style="padding: 30px;">
                        <form id="form" runat="server" method="post">
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <h2 class="m-0 font-weight-bold text-primary">Comprobantes</h2><hr />
                                    <div>
                                        <asp:label ID="lblval" runat="server">
                                            <div runat="server" id="msgerror">
                                                <strong><asp:label runat="server" ID="lblmsg"></asp:label></strong> <asp:label runat="server" ID="lblmsgc"></asp:label>
                                            </div>
                                        </asp:label>
                                    </div>
                                    <div class="form-row">
                                        <div class="form-group col-md-6">
                                            <label>Cliente(*)</label>
                                            <asp:DropDownList ID="cmbcli" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="cmbcli_SelectedIndexChanged">
                                                <asp:ListItem Value="0">&lt;Seleccione un cliente&gt;</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="form-group col-md-6">
                                            <label>Tipo de comprobante(*)</label>
                                            <asp:DropDownList ID="cmbcom" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="cmbcom_SelectedIndexChanged" AppendDataBoundItems="true">
                                                <asp:ListItem Value="0">&lt;Seleccione una opción&gt;</asp:ListItem>
                                                <asp:ListItem Value="1">Boleta</asp:ListItem>
                                                <asp:ListItem Value="2">Factura</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="form-row">
                                        <div class="form-group col-md-6">
                                            <label>Código de cliente</label>
                                            <asp:TextBox ID="txtcodigo" runat="server" ReadOnly="true" CssClass="form-control" />
                                        </div>
                                        <div class="form-group col-md-6">
                                            <label>Cliente</label>
                                            <asp:TextBox ID="txtcli" runat="server" ReadOnly="true" CssClass="form-control" />
                                        </div>
                                        <div class="form-group col-md-6">
                                            <label>DNI</label>
                                            <asp:TextBox ID="txtdni" runat="server" ReadOnly="true" CssClass="form-control" />
                                        </div>
                                        <div class="form-group col-md-6">
                                            <label>Dirección</label>
                                            <asp:TextBox ID="txtdir" runat="server" ReadOnly="true" CssClass="form-control" />
                                        </div>
                                    </div>
                                    <hr />
                                    <div class="table-responsive">
                                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-bordered table-striped table-hover" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="codigo" HeaderText="Código venta" SortExpression="codigo" />
                                                <asp:BoundField DataField="serieComprobante" HeaderText="N° comprobante" SortExpression="serieComprobante" />
                                                <asp:BoundField DataField="fechaVenta" DataFormatString="{0:d}" HeaderText="Fecha" SortExpression="fechaVenta" />
                                                <asp:BoundField DataField="nombrePro" HeaderText="Producto" SortExpression="nombrePro" />
                                                <asp:BoundField DataField="precioVenta" HeaderText="Precio unitario" SortExpression="precioVenta" />
                                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                                <asp:BoundField DataField="monto" DataFormatString="{0:n}" HeaderText="Monto" SortExpression="monto" />
                                            </Columns>
                                            <HeaderStyle CssClass="thead-dark"/>
                                            <footerstyle/>
                                        </asp:GridView>
                                    </div>
                                    <div class="col-sm-4 form-row float-right">
                                        <div class="form-group col-md-6">
                                            <label class="float-right font-weight-bold"><b>Subtotal</b></label>
                                        </div>
                                        <div class="form-group col-md-6">
                                            <asp:TextBox ID="txtsub" runat="server" CssClass="form-control" ReadOnly="true"/>
                                        </div>
                                        <div class="form-group col-md-6">
                                            <label class="float-right font-weight-bold"><b>IGV</b></label>
                                        </div>
                                        <div class="form-group col-md-6">
                                            <asp:TextBox ID="txtigv" runat="server" CssClass="form-control" ReadOnly="true"/>
                                        </div>
                                        <div class="form-group col-md-6">
                                            <label class="active float-right font-weight-bold"><b>Total</b></label>
                                        </div>
                                        <div class="form-group col-md-6">
                                            <asp:TextBox ID="txttot" runat="server" CssClass="form-control" ReadOnly="true"/>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                             <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                <div class="modal-dialog" role="document">
                                    <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">¿Desea cerrar sesión?</h5>
                                        <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                        <span aria-hidden="true">×</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">Seleccione "Cerrar sesión" para salir de su cuenta.</div>
                                    <div class="modal-footer">
                                        <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancelar</button>
                                        <asp:Button runat="server" Text="Cerrar sesión" CssClass="btn btn-primary" ID="btnsesion" OnClick="btnsesion_Click"/>
                                    </div>
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>

            <footer class="sticky-footer bg-white">
                <div class="container my-auto">
                    <div class="copyright text-center my-auto">
                    <span>Copyright &copy; Condori - Gutierrez - Huaman 2020</span>
                    </div>
                </div>
            </footer>
        </div>
    </div>

    <a class="scroll-to-top rounded" href="#page-top">
    <i class="fas fa-angle-up"></i>
    </a>

    <script src="/Content/vendor/jquery/jquery.min.js"></script>
    <script src="/Content/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="/Content/vendor/jquery-easing/jquery.easing.min.js"></script>
    <script src="/Content/js/sb-admin-2.min.js"></script>
    <script src="/Content/js/sb-admin-2.val.js"></script>
    <script src="/Content/vendor/chart.js/Chart.min.js"></script>
    <script src="/Content/js/demo/chart-area-demo.js"></script>
    <script src="/Content/js/demo/chart-pie-demo.js"></script>
</body>
</html>




