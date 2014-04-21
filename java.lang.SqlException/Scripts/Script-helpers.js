function ShowGenericModal(title, message) {
    var resultDialog = $('<div id="resultDialog">' + message + '</div>');

    resultDialog.dialog({
        modal: true,
        title: title,
        resizable: true,
        buttons: {
            Ok: function () {
                $(this).dialog('close');
            }
        }
    });
}