
var table = $("#tblCustomer").DataTable({
    "processing": true,
    "serverSide": true,
    ajax: {
        url: "/api/customer/readDatatable",
        dataSrc: 'data',
        dataType: 'json'
    },
    columns: [
        { data: "name" },
        { data: "remark"},
    ]
});

function showAddEmployeeForm() {

}

function reload() {
    table.ajax.reload();
    notify("fa fa-check","Berhasil", "Data berhasil di reload", "success");
}

$(document).ready(function () {

});
