var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "/api/payrollDetail/readDatatable/"+$("#payrollHistoryId").val(),
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        { "data": "employee.nik" },
        { "data": "employee.name" },
        { "data": "netto" },
        { "data": "payrollDetailStatus" },
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showEditForm('" + row.id + "'); >Detail</button>";
            }
        },
    ]
});


function showAddPayrollDetailForm() {
    $('#addPayrollDetailModal').modal('show');
}

function showEditForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/payrollDetail/readDetail/" + id,
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
    $('#editPayrollDetailModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
    notify("fa fa-check", "Berhasil", "Data berhasil di reload", "success");
}

function updatePayrollDetail() {
    var fd = new FormData();
    var files = $('#fileUpload1')[0].files[0];
    fd.append('file', files);
    $.ajax({
        url: 'api/payrollDetail/update/' + $("#payrollHistoryId").val(),
        data: fd,
        processData: false,
        contentType: false,
        type: 'post',
        success: function (response) {
            reload()
            $('#addEmployeeModal').modal('hide');
            console.log('success', response);
        },
        error: function (result) {
            console.log('error', result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}



$(document).ready(function () {
    $('.select2addmodal').select2({
        dropdownParent: $('#addPayrollDetailModal')
    });
});
