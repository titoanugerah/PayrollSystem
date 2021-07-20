var table = $("#table").DataTable({
    "processing": true,
    "serverSide": true,
    "filter": true,
    "ordering": false,
    "paging": true,
    "pageLength": 10,
    "ajax": {
        "url": "api/payrollDetail/readPersonalDatatable",
        "dataSrc": "data",
        "type": "POST",
        "dataType": 'json'
    },
    "columns": [
        {
            "render": function (data, type, row) {
                return row.payrollHistory.month + " " + row.payrollHistory.year;
            }
        },
        {
            "render": function (data, type, row) {
                return "Rp. " + row.takeHomePay.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            }
        },
        //{ "data": "payrollDetailStatus" },
        {
            "render": function (data, type, row) {
                return "<button type='button' class='btn btn-info' onclick=showDetailForm('" + row.id + "'); >Detail</button>";
            }
        },
    ]
});

function showDetailForm(id) {
    $.ajax({
        type: "GET",
        dataType: "JSON",
        url: "api/payrollDetail/readDetail/" + id,
        success: function (result) {
            console.log(result);
            $('#name').val(result.employee.name);
            $('#nik').val(result.employee.nik);
            $('#mainSalaryBilling').val(formatter.format(result.mainSalaryBilling));
            $('#insentiveBilling').val(formatter.format(result.insentiveBilling));
            $('#attendanceBilling').val(formatter.format(result.attendanceBilling));
            $('#overtimeBilling').val(formatter.format(result.overtimeBilling));
            $('#apreciationBilling').val(formatter.format(result.appreciationBilling));
            $('#bpjsTkDeduction').val(formatter.format(result.bpjsTkDeduction));
            $('#bpjsKesehatanDeduction').val(formatter.format(result.bpjsKesehatanDeduction));
            $('#pensionDeduction').val(formatter.format(result.pensionDeduction));
            $('#pph21').val(formatter.format(result.ppH21));
            $('#anotherDeduction').val(formatter.format(result.anotherDeduction));
            $('#transferFee').val(formatter.format(result.transferFee));
            $('#takeHomePay').val(formatter.format(result.takeHomePay));
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
    notify("fa fa-check", "Berhasil", "Data berhasil di reload", "success");
}

var formatter = new Intl.NumberFormat('id-ID', {
    style: 'currency',
    currency: 'IDR',
});
    