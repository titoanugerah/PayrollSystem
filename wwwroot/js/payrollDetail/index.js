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
        {
            "render": function (data, type, row) {
                return zeroPad(row.employee.nik);
            }
        },
        { "data": "employee.name" },
        { "data": "employee.location.name" },
        {
            "render": function (data, type, row) {
                return "Rp. " + row.takeHomePay.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            }
        },
        { "data": "payrollDetailStatus" },
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showDetailForm('" + row.id + "'); >Detail</button> &nbsp;&nbsp;" + "<a href='PayrollDetail/Download/Slip/" + row.id + "' class='btn btn-info' target='_blank'>Download</a> &nbsp;&nbsp;"  + "<button type='button' onclick='deletePayrollDetail(" + row.id + ")' class='btn btn-danger'>Hapus</button>";
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
    $('#downloadReportModal').modal('hide');
}

function downloadSlip() {
    window.open('PayrollDetail/Download/Slip/' + $("#id").val() , '_blank');
}

function downloadBankReport() {
    window.open('PayrollHistory/Download/ReportBank/' + $("#payrollHistoryId").val(), '_blank');    
}

function deletePayrollDetail(id) {
    $.ajax({
        type: "POST",
        dataType: "JSON",
        url: "api/payrollDetail/delete/"+id,
        success: function (result) {
            notify('fas fa-check', 'Sukses', "data berhasil dihapus", 'success');
            reloadTable();
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.responseText, 'danger');
        }
    });
}


function showAddPayrollDetailForm() {
    $('#addPayrollDetailModal').modal('show');
}

function resync() {
    var url = "api/payrollDetail/resync/" + $('#payrollHistoryId').val();
    $.ajax({
        type: "POST",
        dataType: "JSON",
        url: url,
        success: function (result) {
            reloadTable();
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.responseText, 'danger');
        }
    });
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

var formatter = new Intl.NumberFormat('id-ID', {
    style: 'currency',
    currency: 'IDR',
});


function showDetailForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/payrollDetail/readDetail/" + id,
        success: function (result) {
            console.log(result);
            $('#name').val(result.employee.name);
            $('#id').val(result.id);
            $('#nik').val(zeroPad(result.employee.nik));
            $('#mainSalaryBilling').val(formatter.format(result.mainSalaryBilling));
            $('#overtimeBilling').val(formatter.format(result.overtimeBilling));
            $('#attendanceBilling').val(formatter.format(result.attendanceBilling));
            $('#insentiveBilling').val(formatter.format(result.insentiveBilling));
            $('#rapel').val(result.rapel);
            $('#apreciationBilling').val(formatter.format(result.appreciationBilling));
            $('#bpjsTkDeduction').val(formatter.format(result.bpjsTkDeduction));
            $('#bpjsKesehatanDeduction').val(formatter.format(result.bpjsKesehatanDeduction));
            $('#pensionDeduction').val(formatter.format(result.pensionDeduction));
            $('#absentDeduction').val(result.absentDeduction);
            $('#pph21').val(formatter.format(result.ppH21));
            $('#anotherDeduction').val(result.anotherDeduction);
            $('#transferFee').val(result.transferFee);
            $('#takeHomePay').val(formatter.format(result.takeHomePay));
            if (result.payrollDetailStatusId == 3) {
                $("#updateBtn").hide();
            }
        },
        error: function (result) {
            console.log(result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
        $('#payrollDetailModal').modal('show');
}

function reloadTable() {
    table.ajax.reload();
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
            console.log('success', response);
            $('#addPayrollDetailModal').modal('hide');
        },
        error: function (result) {
            console.log('error', result);
            $('#addPayrollDetailModal').modal('hide');
            if (result.responseText == "0") {
                $('#submitErrorModal').modal('show');
            }
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function showSubmitForm() {
    $.ajax({
        url: 'api/PayrollHistory/check/' + $("#payrollHistoryId").val(),
        type: 'post',
        success: function (response) {
            reloadTable()
            $("#submitModal").modal('show');
        },
        error: function (result) {
            console.log('error', result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function submit(isLateSubmit) {
    $.ajax({
        url: 'api/PayrollHistory/submit/' + $("#payrollHistoryId").val(),
        type: 'post',
        data: {
            IsLateTransfer: isLateSubmit
        },
        success: function (response) {
            reloadTable()
            $("#submitModal").modal('hide');
        },
        error: function (result) {
            console.log('error', result);
            $("#submitModal").modal('hide');
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function updateDetail() {
    
    $.ajax({
        url: 'api/PayrollDetail/updateDetail/' + $("#id").val(),
        type: 'post',
        dataType: "json",
        data: {
            Rapel : $("#rapel").val(),
            TransferFee : $("#transferFee").val(),
            AnotherDeduction : $("#anotherDeduction").val(),
        },
        success: function (response) {
            reloadTable()
        },
        error: function (result) {
            console.log('error', result);
            notify('fas fa-times', 'Gagal', result.statusText + ' &nbsp; ' + result.responseText, 'danger');
        }
    });
}

function zeroPad(num) {
    return num.toString().padStart(4, "0");
}

$(document).ready(function () {
    $('.select2addmodal').select2({
        dropdownParent: $('#addPayrollDetailModal')
    });
    $('.select2modal').select2({
        dropdownParent: $('#downloadReportModal')
    });
});
