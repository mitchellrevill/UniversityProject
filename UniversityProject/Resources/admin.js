
const departmentData = [
    { DepartmentId: 1, DepartmentName: "Human Resources", Description: "Manages employee relations and resources." },
    { DepartmentId: 2, DepartmentName: "Finance", Description: "Handles budgeting, accounting, and investments." },
    { DepartmentId: 3, DepartmentName: "IT Support", Description: "Provides technical support and infrastructure management." }
];
populateDepartmentTable(departmentData)
function populateDepartmentTable(data) {
    const tbody = document.getElementById('departmentTableBody'); // Ensure you have the correct ID for the tbody
    tbody.innerHTML = ''; // Clear existing rows

    data.forEach(item => {
        const row = document.createElement('tr');

        row.innerHTML = `
            <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.DepartmentId}" data-name="${item.DepartmentName}" data-description="${item.Description}"></td>
            <td>${item.DepartmentId}</td>
            <td>${item.DepartmentName}</td>
            <td>${item.Description}</td>
        `;

        tbody.appendChild(row);
    });
}
function populateRegionTable(data) {
    const tbody = document.getElementById('regionTableBody'); // Ensure you have the correct ID for the tbody
    tbody.innerHTML = ''; // Clear existing rows

    data.forEach(item => {
        const row = document.createElement('tr');

        row.innerHTML = `
            <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.RegionId}" data-name="${item.RegionName}" data-country-id="${item.CountryId}"></td>
            <td>${item.RegionId}</td>
            <td>${item.RegionName}</td>
            <td>${item.CountryId}</td>
        `;

        tbody.appendChild(row);
    });
}

function populateCountryTable(data) {
    const tbody = document.getElementById('countryTableBody');
    tbody.innerHTML = ''; 

    data.forEach(item => {
        const row = document.createElement('tr');

        row.innerHTML = `
            <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.CountryId}" data-name="${item.CountryName}" data-currency="${item.CountryCurrency}" data-minimum-leave="${item.MinimumLeave}"></td>
            <td>${item.CountryId}</td>
            <td>${item.CountryName}</td>
            <td>${item.CountryCurrency}</td>
            <td>${item.MinimumLeave}</td>
            <td>${item.LegalRequirements.join(", ")}</td>
        `;

        tbody.appendChild(row);
    });
}
function getSelectedIds() {
    const selectedIds = [];
    document.querySelectorAll(".dynamic-checkbox-item:checked").forEach((checkbox) => {
        selectedIds.push(checkbox.dataset.id);  // Collect selected IDs
    });
    return selectedIds;
}

const host = `${window.location.protocol}//${window.location.host}`;

function addNewDepartment() {
    console.log("Method Start");
    var DepartmentId = Math.floor(Math.random() * 1000) + 1;
    var DepartmentTitle = document.getElementById("departmentName").value;
    var DepartmentDescription = document.getElementById("DepartmentDescription").value;

    console.log(DepartmentId)
    if (!DepartmentTitle || !DepartmentDescription) {
        alert("You have not answered all required fields");
        return;
    }

    var Department = {
        "DepartmentId": DepartmentId,
        "DepartmentName": DepartmentTitle,
        "Description": DepartmentDescription,
    };
    FetchRequest('InsertDepartment', Department);
}

function updateDepartment() {
    console.log("Req Rec")
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No departments selected for updating.");
        return;
    }
    console.log("Req Rec")
    selectedIds.forEach(element => {
        var DepartmentId = element;
        var DepartmentTitle = document.getElementById("departmentNameUpdate").value;
        var DepartmentDescription = document.getElementById("departmentDescriptionUpdate").value;

        console.log(element)

        var DepartmentPosting = {
            "DepartmentId": DepartmentId,
            "DepartmentName": DepartmentTitle,
            "Description": DepartmentDescription,
        };
        console.log(DepartmentPosting)
        FetchRequest('UpdateDepartment', DepartmentPosting);
    });
}

async function DeleteDepartment() {
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No departments selected for deletion.");
        return;
    }

    selectedIds.forEach(element => {
        var DepartmentId = element;

        var DepartmentPosting = {
            "DepartmentId": DepartmentId,
            "DepartmentName": "NULL",
            "Description": "NULL",
        };

        FetchRequest('DeleteDepartment', DepartmentPosting);
    });
}



function addNewCountry() {
    console.log("Method Start");
    var CountryId = document.getElementById("countryId").value;
    var CountryName = document.getElementById("countryName").value;
    var CountryCurrency = document.getElementById("countryCurrency").value;
    var LegalRequirements = document.getElementById("legalRequirements").value;
    var MinimumLeave = parseInt(document.getElementById("minimumLeave").value);

    if (!CountryId || !CountryName || !CountryCurrency || !LegalRequirements || isNaN(MinimumLeave)) {
        alert("You have not answered all required fields");
        return;
    }

    var Country = {
        "CountryId": CountryId,
        "CountryName": CountryName,
        "Currency": CountryCurrency,
        "LegalRequirements": LegalRequirements.split(',').map(item => item.trim()), // Convert to array
        "MinimumLeave": MinimumLeave
    };

    FetchRequest('InsertCountry', Country);
}
function updateCountry() {
    console.log("Request Received");
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No countries selected for updating.");
        return;
    }

    selectedIds.forEach(element => {
        var CountryId = element;
        var CountryName = document.getElementById("countryNameUpdate").value;
        var CountryCurrency = document.getElementById("countryCurrencyUpdate").value;
        var LegalRequirements = document.getElementById("legalRequirementsUpdate").value;
        var MinimumLeave = parseInt(document.getElementById("minimumLeaveUpdate").value);

        console.log(element);

        var CountryPosting = {
            "CountryId": CountryId,
            "CountryName": CountryName,
            "Currency": CountryCurrency,
            "LegalRequirements": LegalRequirements.split(',').map(item => item.trim()), 
            "MinimumLeave": MinimumLeave
        };
        console.log(CountryPosting);
        FetchRequest('UpdateCountry', CountryPosting);
    });
}
async function DeleteCountry() {
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No countries selected for deletion.");
        return;
    }

    selectedIds.forEach(element => {
        var CountryId = element;

        var CountryPosting = {
            "CountryId": CountryId,
            "CountryName": "NULL",
            "Currency": "NULL",
            "LegalRequirements": [],
            "MinimumLeave": 0
        };

        FetchRequest('DeleteCountry', CountryPosting);
    });
}

function addNewRegion() {
    console.log("Method Start");
    var RegionId = document.getElementById("regionId").value;
    var RegionName = document.getElementById("regionName").value;
    var CountryId = document.getElementById("countryId").value;

    if (!RegionId || !RegionName || !CountryId) {
        alert("You have not answered all required fields");
        return;
    }

    var Region = {
        "RegionId": RegionId,
        "RegionName": RegionName,
        "CountryId": CountryId
    };

    FetchRequest('InsertRegion', Region);
}




function updateRegion() {
    console.log("Request Received");
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No regions selected for updating.");
        return;
    }

    selectedIds.forEach(element => {
        var RegionId = element;
        var RegionName = document.getElementById("regionNameUpdate").value;
        var CountryId = document.getElementById("countryIdUpdate").value;

        console.log(element);

        var RegionPosting = {
            "RegionId": RegionId,
            "RegionName": RegionName,
            "CountryId": CountryId
        };
        console.log(RegionPosting);
        FetchRequest('UpdateRegion', RegionPosting);
    });
}
async function DeleteRegion() {
    const selectedIds = getSelectedIds();
    if (selectedIds.length === 0) {
        alert("No regions selected for deletion.");
        return;
    }

    selectedIds.forEach(element => {
        var RegionId = element;

        var RegionPosting = {
            "RegionId": RegionId,
            "RegionName": "NULL",
            "CountryId": "NULL"
        };

        FetchRequest('DeleteRegion', RegionPosting);
    });
}

async function FetchRequest(uri, DepartmentModel) {
    console.log("Req sent")
    try {
        const response = await fetch(host + '/' + uri, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(DepartmentModel)
        });

        if (response.ok) {
            const data = await response.text();
            alert(data);
        } else {
            throw new Error('Error performing operation on the Department');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation on the Department');
    }
}