$(() => {

    let id = $("#image-id").val();

    setInterval(() => {
        $.get("/home/GetLikes", { id }, function (likes) {
            $("#likes-count").text(likes);
        })
    }, 500);

    $("#like-button").on("click", function () {
        $.post("/home/Update", { id }, function () {
            $("#like-button").attr("disabled", true);
        });
    });
});