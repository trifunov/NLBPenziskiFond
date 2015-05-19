function onComplete(e) {
    $(".t-icon.t-refresh").removeClass("t-loading");
}


function onBinding(e) {
    $(".t-icon.t-refresh").addClass("t-loading");
}

$(document).ready(function () {

    $("input[type='checkbox']").on("change", function () {
        var idTarget = $(this).attr("data-target");
        if ($(this).prop('checked')) {
            $("#" + idTarget).val(true);
        } else {
            $("#" + idTarget).val(false);
        }
    });
});