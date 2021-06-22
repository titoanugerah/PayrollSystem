﻿
var table = $("#tblEmployee").DataTable({
    "processing": true,
    "serverSide": true,
    ajax: {
        url: "/api/employee/readDatatable",
        dataSrc: 'data',
        dataType: 'json'
    },
    columns: [
        { data: "nik" },
        { data: "name"},
        { data: "customer"},
        { data: "position"},
        { data: "location"},
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
