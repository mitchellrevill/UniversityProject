document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("addDepartmentForm");
    const container = document.getElementById("container");
    const openModalLink = document.getElementById("openModal");

    // Function to close the modal
    function closeModal() {
        container.style.visibility = "hidden";
        container.style.display = "none";
    }

    // Function to open the modal
    function openModal() {
        container.style.visibility = "visible";
        container.style.display = "block";
    }

    // Open the modal when the "Reveal" link is clicked
    openModalLink.addEventListener("click", function (event) {
        event.preventDefault(); // Prevent default anchor behavior
        openModal();
    });

    // Close the modal when the form is submitted
    form.addEventListener("submit", function (event) {
        event.preventDefault(); 
        closeModal();
    });
});
