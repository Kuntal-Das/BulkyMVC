document.addEventListener("DOMContentLoaded", LoadTable);
var datatable;
function LoadTable() {
    datatable = $('#products_table').DataTable({
        "ajax": { url: '/admin/product/all' },
        "columns": [
            { "title": "Title", "data": "title", "width": "10%" },
            { "title": "ISBN", "data": "isbn", "width": "10%" },
            { "title": "Listed price", "data": "listedPrice", "width": "10%" },
            { "title": "Author", "data": "author", "width": "10%" },
            { "title": "Category", "data": "category.name", "width": "10%" },
            {
                "title": "Actions",
                "data": "id",
                "width": "10%",
                "render": (id) => `
                    <div class="row">
                        <a class="link-body-emphasis col-6 text-center" href="./Product/Upsert?id=${id}">
                            <i class="bi bi-pencil-square" style="color:royalblue;"></i>
                        </a>
                        <a class="link-body-emphasis col-6 text-center" onClick=DeleteOnConfirm(${id})>
                            <i class="bi bi-trash-fill" style="color:crimson;"></i>
                        </a>
                    <div>`
            }
        ]
    });
}

async function DeleteOnConfirm(id) {
    var result = await Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    });

    if (result.isConfirmed) {
        url = `${window.location.href}/Delete?id=${id}`;
        var response = await fetch(
            url, {
            method: 'DELETE',
            headers: {
                "Content-Type": "application/json"
            }
        });
        if (response.ok) {
            datatable.ajax.reload();
            var json = await response.json()
            toastr["success"](json.message);
        } else {
            datatable.ajax.reload();
            var json = await response.json()
            toastr["error"](json.message);
        }
  
    }
}