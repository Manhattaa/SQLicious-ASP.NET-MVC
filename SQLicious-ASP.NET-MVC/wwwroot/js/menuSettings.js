//function toggleEdit(itemId) {
//    var viewRow = document.getElementById("view-row-" + itemId);
//    var editRow = document.getElementById("edit-row-" + itemId);

//    if (viewRow.style.display === "none") {
//        viewRow.style.display = "";
//        editRow.style.display = "none";
//    } else {
//        viewRow.style.display = "none";
//        editRow.style.display = "";
//    }
//}
//function generatePdf(menuType) {
//    document.getElementById("spinner").style.display = "block";

//    fetch(`/api/MenuItem/generatepdf/${menuType}`, {
//        method: 'POST'
//    })
//        .then(response => {
//            if (!response.ok) {
//                throw new Error("Network response was not ok");
//            }
//            return response.json();
//        })
//        .then(data => {
//            if (data && data.pdfUrl) {
//                document.getElementById("spinner").style.display = "none";
//                document.getElementById("confirmationMessage").innerHTML = `
//                            <div class="alert alert-success">
//                                PDF for ${menuType} generated successfully! <a href="${data.pdfUrl}" target="_blank">View PDF</a>
//                            </div>`;
//            } else {
//                throw new Error("Failed to generate PDF.");
//            }
//        })
//        .catch(error => {
//            document.getElementById("spinner").style.display = "none";
//            document.getElementById("confirmationMessage").innerHTML = `
//                        <div class="alert alert-danger">Failed to generate PDF for ${menuType}</div>`;
//            console.error(error);
//        });


//    window.filterMenu = function (menuType) {
//        var sections = document.querySelectorAll(".menu-section");

//        sections.forEach(function (section) {
//            var sectionMenuType = section.getAttribute('data-menutype');

//            if (menuType === 'All' || sectionMenuType === menuType) {
//                section.style.display = "block"; // Show matching section
//            } else {
//                section.style.display = "none";  // Hide non-matching sections
//            }
//        });
//    };
//}