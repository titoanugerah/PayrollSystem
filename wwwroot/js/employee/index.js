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


$(document).ready(function () {
    //tabless.ajax.reload();
});
//$(document).ready(function () {
//    $("#example").DataTable({
//        "processing": true, // for show progress bar    
//        "serverSide": true, // for process server side    
//        "filter": true, // this is for disable filter (search box)    
//        "orderMulti": false, // for disable multiple column at once    
//        "ajax": {
//            "url": "api/employee/read",
//            "type": "POST",
//            "datatype": "json"
//        },
//        "columnDefs": [{
//            "targets": [0],
//            "visible": false,
//            "searchable": false
//        }],
//        "columns": [
//            { "data": "nik", "name": "NIK", "autoWidth": true },
//            { "data": "name", "name": "Nama", "autoWidth": true },
//            { "data": "position", "name": "Jabatan", "autoWidth": true },
//            { "data": "customer", "name": "Customer", "autoWidth": true },
//            { "data": "location", "name": "Area", "autoWidth": true },
//            //{ "data": "NIK", "name": "Opsi", "autoWidth": true },
//        ]

//    });
//});


//function DeleteData(CustomerID) {
//    if (confirm("Are you sure you want to delete ...?")) {
//        Delete(CustomerID);
//    } else {
//        return false;
//    }
//}


//function Delete(CustomerID) {
//    var url = '@Url.Content("~/")' + "DemoGrid/Delete";

//    $.post(url, { ID: CustomerID }, function (data) {
//        if (data) {
//            oTable = $('#example').DataTable();
//            oTable.draw();
//        } else {
//            alert("Something Went Wrong!");
//        }
//    });
//}  