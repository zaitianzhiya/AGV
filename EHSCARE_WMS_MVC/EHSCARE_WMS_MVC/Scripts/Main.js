$(document).ready(function () {
    //$('.ajaxGrid table thead tr a').on('click', updateGrid);
    $('.ajaxGrid table tfoot tr a').on('click', updateGrid);
    $("#divGrid table tbody tr").on("click", function () {
        $(".selectedRow").removeClass("selectedRow");
        $(this).addClass("selectedRow");
    });
});

function updateGrid(e) {
    e.preventDefault();
    var url = $(this).attr('href');
    var grid = $(this).parents('.ajaxGrid');
    var id = grid.attr('id');
    grid.load(url + ' #' + id);
};

function BindClick() {
    $("#divGrid table tbody tr").each(function () {
        $(this).on("click", function () {
            $(".selectedRow").removeClass("selectedRow");
            $(this).addClass("selectedRow");
        });
    });
}