function openModal(modalId) {
    var modalElement = document.getElementById(modalId);
    if (modalElement) {
        var modal = new bootstrap.Modal(modalElement);
        modal.show();
    } else {
        console.error(`Modal with ID '${modalId}' not found.`);
    }
}

function closeModal(modalId) {
    var modalElement = document.getElementById(modalId);
    if (modalElement) {
        var modal = bootstrap.Modal.getInstance(modalElement);
        if (modal) {
            modal.hide();
        } else {
            console.error(`Bootstrap modal instance for ID '${modalId}' not found.`);
        }
    } else {
        console.error(`Modal with ID '${modalId}' not found.`);
    }
}
