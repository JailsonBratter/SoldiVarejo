<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgendaPrint.aspx.cs" Inherits="visualSysWeb.modulos.PetShop.pages.AgendaPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <title>Print Agendamento</title>
</head>
<script>
    function printRelatorio() {
        document.getElementById("imgPrint").style.visibility = "hidden";
        self.print();
        document.getElementById("imgPrint").style.visibility = "visible";
    } 
</script>


<body onload="self.print();">
    <form id="form1" runat="server">
    <h1><center>Agendamento</center></h1>
    
    
    <hr />
    <div style="width:200px; float:left;" >
    Pedido :<asp:Label ID="lblPedido" runat="server" Text="Label"></asp:Label> 
    </div>
    <div style="width:400px; float:left;" >
    Usuario: <asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label> 
    </div>
    <br />
    
    Nome Funcionario: <asp:Label ID="lblFuncionario" runat="server" Text="Label"></asp:Label><br />
    Cliente :  <asp:Label ID="lblCliente" runat="server" Text="Label"></asp:Label><br />
    Nome Pet: <asp:Label ID="lblPet" runat="server" Text="Label"></asp:Label><br />
    <div style="width:200px; float:left;" >
    Data: <asp:Label ID="lblData" runat="server" Text="Label"></asp:Label> 
    </div>
    <div style="width:200px; float:left;" >
    Horario: <asp:Label ID="lblInicio" runat="server" Text="00:00"></asp:Label> Ate <asp:Label ID="lblFim" runat="server" Text="00:00"></asp:Label>
    </div>
    <br />

    Delivery: <asp:Label ID="lblDelivery" runat="server" Text="SIM"></asp:Label> <br />
    <hr />
    <h3>Retirada </h3>
    <hr />
    <div style="width:200px; float:left;" >
    Hora:<asp:Label ID="lblRetirada" runat="server" Text=""></asp:Label>
    </div>
    <div style="width:200px; float:left;" >
    Funcionario: <asp:Label ID="lblFuncionarioRetira" runat="server" Text=""></asp:Label><br />
    </div>
    <br />
    <hr />
    <h3>Entrega</h3>
    <hr />
    <div style="width:200px; float:left;" >
    Prevista:<asp:Label ID="lblHoraPrevista" runat="server" Text="00:00"></asp:Label>
    </div>
    <div style="width:200px; float:left;" >
    Real:<asp:Label ID="lblHoraReal" runat="server" Text="00:00"></asp:Label><br />
    </div>
    Funcionario: <asp:Label ID="lblFuncionarioEntrega" runat="server" Text="Funcionario Entrega"></asp:Label><br />

    <div style="width:200px; float:left;" >
    Km Saida: <asp:Label ID="lblKmSaida" runat="server" Text=""></asp:Label>
    </div>
    <div style="width:200px; float:left;" >
    Km Chegada:<asp:Label ID="lblKmChegada" runat="server" Text=""></asp:Label>
    </div>
    <br />

    <hr />
    <h3>Serviços</h3>
    <hr />
    <asp:Label ID="lblServicos" runat="server" Text="Servicos"></asp:Label><br />
    <hr />
    <h3>Observações</h3>
    <hr />
    <asp:Label ID="lblObservacoes" runat="server" Text="Observacoes"></asp:Label><br />
    <hr />
    <h3>Observações Veterinario</h3>
    <hr />
    <asp:Label ID="lblObsVeterinario" runat="server" Text="Obs Veterinario"></asp:Label><br />




    
    
    </form>
</body>
</html>
