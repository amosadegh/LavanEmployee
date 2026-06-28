var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/employees/getall' },
        "columns": [
            { data: 'employeeNumber', "width": "10%" },
            { data: 'name', "width": "15%" },
            { data: 'family', "width": "15%" },
            { data: 'nationalNumber', "width": "15%" },
            { data: 'job.jobTitle', "width": "15%" },
            { data: 'organization.organizationTitle', "width": "15%" },
            {
                data: 'empId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/employees/edit?id=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-square"></i> ویرایش
                        </a>
                        <a onClick=Delete('/admin/employees/delete/${data}') class="btn btn-danger mx-2">
                            <i class="bi bi-trash-fill"></i> حذف
                        </a>
                    </div>`;
                },
                "width": "15%"
            }
        ],
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.4/i18n/fa.json"
        }
    });
}

function Delete(url) {
    Swal.fire({
        title: 'آیا مطمئن هستید؟',
        text: "این عملیات قابل بازگشت نیست!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله، حذف شود!',
        cancelButtonText: 'انصراف'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}

