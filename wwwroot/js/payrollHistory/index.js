var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/payrollHistory/readDatatable",
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        {
            "render": function (data, type, row) {
                return  row.month + ", " + row.year;
            }
        },
        { "data": "status.name" },
        { "data": "status.remark" },
        {
            "render": function (data, type, row) {
                return "<a href='payrollDetail/"+row.id+"' class='btn btn-info' >Detail</a>";
            }
        },
    ]
});

function showAddPayrollHistoryForm() {
    $('#addPayrollHistoryModal').modal('show');
}

function showEditForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/payrollHistory/readDetail/" + id,
        success: function (result) {
            console.log(result);
            $('#editId').val(result.id);
            $('#editName').val(result.name);
            $('#editRemark').val(result.remark);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
    $('#editPayrollHistoryModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    //getDeletedPayrollHistory();
    notify("fa fa-check", "Berhasil", "Data berhasil di reload", "success");
}

function addPayrollHistory() {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        contentType: "application/x-www-form-urlencoded",
        url: "api/payrollHistory/create",
        data: {
            Month: $("#addMonth").val(),
            Year: $("#addYear").val(),
        },
        success: function (result) {
            $('#addPayrollHistoryModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'PayrollHistory berhasil ditambahkan', 'success');
            reloadTable();
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}


function updatePayrollHistory() {
    $.ajax({
        type: "POST",
        dataType: "json",
        contentType: "application/x-www-form-urlencoded",
        url: "api/payrollHistory/update/" + $('#editId').val(),
        data: {
            Name: $("#editName").val(),
            Remark: $("#editRemark").val(),
        },
        success: function (result) {
            $('#editPayrollHistoryModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'PayrollHistory berhasil diubah', 'success');
            reloadTable();
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

//function deletePayrollHistory() {
//    $.ajax({
//        type: "POST",
//        contentType: 'application/json; charset=utf-8',
//        //dataType: "JSON",
//        url: "api/payrollHistory/delete/" + $('#editId').val(),
//        success: function (result) {
//            reloadTable();
//            $('#editPayrollHistoryModal').modal('hide');
//            notify('fas fa-check', 'Berhasil', 'PayrollHistory berhasil dihapus', 'success');
//        },
//        error: function (result) {
//            console.log(result);
//            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
//        }
//    });
//}

function recoverPayrollHistory() {
    $.ajax({
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        //dataType: "JSON",
        url: "api/payrollHistory/recover/" + $('#recoverId').val(),
        success: function (result) {
            reloadTable();
            $('#addPayrollHistoryModal').modal('hide');
            notify('fas fa-check', 'Berhasil', 'PayrollHistory berhasil dipulihkan', 'success');
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function getDeletedPayrollHistory() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/payrollHistory/readDeleted",
        success: function (result) {
            var html = "<option value=0> Silahkan Pilih</option>";
            result.forEach(function (data) {
                html = html + "<option value=" + data.id + "> " + data.name + "</option>";
            });
            $('#recoverId').html(html);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.responseText, 'danger');
        }
    });
}

$(document).ready(function () {
    $('.select2addmodal').select2({
        dropdownParent: $('#addPayrollHistoryModal')
    });
    //getDeletedPayrollHistory();
});

