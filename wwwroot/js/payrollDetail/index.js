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
        {
            "render": function (data, type, row) {
                return "Rp. " + row.takeHomePay.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            }
        },
        { "data": "payrollDetailStatus" },
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showEditForm('" + row.id + "'); >Detail</button>";
            }
        },
    ]
});

function downloadReport() {
    if ($('#districtId').val() == 0) {
        window.open('PayrollHistory/Download/Report/' + $("#payrollHistoryId").val(), '_blank');
    } else {
        window.open('PayrollHistory/Download/Report/' + $("#payrollHistoryId").val() + '/' + $('#districtId').val(), '_blank');
    }
}

function showAddPayrollDetailForm() {
    $('#addPayrollDetailModal').modal('show');
}

function showDownloadReportForm() {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/district/read",
        success: function (result) {
            var html = "<option value=0> Semua Distrik </option>";
            result.forEach(function (data) {
                html = html + "<option value=" + data.id + "> " + data.name + "</option>";
            });
            $('#districtId').html(html);
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.responseText, 'danger');
        }
    });
    $('#downloadReportModal').modal('show');
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
            reloadTable()
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
    $('.select2modal').select2({
        dropdownParent: $('#downloadReportModal')
    });
});
