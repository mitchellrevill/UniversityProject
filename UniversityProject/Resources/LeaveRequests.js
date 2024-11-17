const host = `${window.location.protocol}//${window.location.host}`;
const token = localStorage.getItem("authToken");
let hoursleft;
var totalHoursUsed;

payload = parseJwt(token)
console.log(payload)
Countries = FetchRequestGET('GetCountries')
Managers = FetchRequestGET('GetManagers')
Employee = GetEmployee()
calculateLeave()



async function populateLeaveRequest() {
    try {
        const leaveRequests = await FetchRequestGET("GetLeaveRequest");
        if (!Array.isArray(Departments)) {
            throw new Error('Expected an array from FetchRequestGET.');
        }

        const tbody = document.getElementById('leaveRequestsTableBody');
        tbody.innerHTML = '';

        if (leaveRequests.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4">No locations available.</td></tr>';
            return;
        }

        leaveRequests.forEach(item => {
            const row = document.createElement('tr');

            row.innerHTML = `
                <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.EmployeeId}" data-name="${item.EmployeeId}" data-description="${item.Description}"></td>
                <td>${item.EmployeeId}</td>
                <td>${item.StartDate}</td>
                <td>${item.EndDate}</td>
                <td>${item.HoursUsed}</td>
                <td>${item.IsApproved}</td>
            `;

            tbody.appendChild(row);
        });
    } catch (error) {
        console.error('Error populating department table:', error);
    }
}

async function submitLeaveRequest() {
    const hoursBetween = calculateHours();
    const leaveDetails = await calculateLeave(); // Await the result
    var employee = await GetEmployee();

    console.log(employee)
    if (leaveDetails) {
        const { totalHoursUsed, hoursLeft } = leaveDetails; // Destructure the object
        console.log(`Total Hours Used: ${totalHoursUsed}`);
        console.log(`Hours Left: ${hoursLeft}`);

        // Use document.getElementById to fetch input values
        const endDate = document.getElementById('endDate').value;
        const startDate = document.getElementById('startDate').value;

        if (hoursLeft - hoursBetween >= 0) {
            const leaverequest = {
                "EmployeeId": employee.EmployeeId,
                "StartDate": startDate,
                "EndDate": endDate,
                "HoursUsed": hoursBetween,
                "IsApproved": "No"
            };


            console.log(leaverequest)
            FetchRequest("InsertLeaveRequest", leaverequest);
        } else {
            alert("Invalid: Exceeding Hour limit");
            return;
        }
    }
}

function calculateHours() {
    const startDateInput = document.getElementById('startDate').value;
    const endDateInput = document.getElementById('endDate').value;

    if (!startDateInput || !endDateInput) {
        alert("Please select both start and end dates.");
        return;
    }

    const startDate = new Date(startDateInput);
    const endDate = new Date(endDateInput);

    if (startDate > endDate) {
        alert("Start date must be earlier than the end date.");
        return;
    }

  
    const millisecondsDifference = endDate - startDate;
    const hoursDifference = millisecondsDifference / (1000 * 60 * 60); // Convert ms to hours

    return hoursDifference;
}

//totalHoursUsed
//hoursLeft




async function calculateLeave() {
    try {
        const leaveRequests = await FetchRequestGET("GetLeaveRequest");
        const Countries = await FetchRequestGET('GetCountries');
        const country = Countries.find((c) => String(c.CountryId) === String(Employee.CountryId));

        const employeeId = Employee.employeeId;
        const currentYear = new Date().getFullYear();

        const filteredRequests = leaveRequests.filter(request => {
            const startDate = new Date(request.StartDate);
            const endDate = new Date(request.EndDate);

            return request.EmployeeId === employeeId &&
                startDate.getFullYear() === currentYear &&
                endDate.getFullYear() === currentYear;
        });

        const totalHoursUsed = filteredRequests.reduce((total, request) => total + (request.HoursUsed || 0), 0);

        const employeeStartDate = new Date(Employee.startDate);
        const isCurrentYearStart = employeeStartDate.getFullYear() === currentYear;
        let remainingLeave = country.MinimumLeave;

        if (isCurrentYearStart) {
            const daysInYear = 365;
            const daysWorked = Math.floor((new Date() - employeeStartDate) / (1000 * 60 * 60 * 24));
            const percentageOfYearWorked = daysWorked / daysInYear;
            const proRatedLeave = employee.totalAnnualLeave * percentageOfYearWorked;
            remainingLeave -= proRatedLeave;
        }

        const hoursLeft = remainingLeave * 24;


        const filterContainer = document.querySelector('.filter-container');
        filterContainer.innerHTML = "";
        const hoursUsedElement = document.createElement('p');
        hoursUsedElement.textContent = `Hours Used This Year: ${totalHoursUsed}`;
        const hoursLeftElement = document.createElement('p');
        hoursLeftElement.textContent = `Hours Left: ${hoursLeft}`;
        filterContainer.appendChild(hoursUsedElement);
        filterContainer.appendChild(hoursLeftElement);
        return { totalHoursUsed, hoursLeft };

    } catch (error) {
        console.error('Error calculating leave:', error);
    }
}



async function GetEmployee() {

        var employee = {
            "EmployeeId": payload.EmployeeId,
            "FirstName": "Default",
            "LastName": "Default",
            "CompanyEmail": "default@company.com",
            "PersonalEmail": "default@personal.com",
            "PhoneNumber": "0000000000",
            "CountryId": 1,             // Default integer values
            "RegionId": 1,
            "DepartmentId": 1,
            "ManagerId": 1,
            "EmploymentType": "Default",
            "StartDate": "2000-01-01",
            "Salary": 0.0,
            "Benefits": "None",
            "Password": "Default",
            "EmployeeType": "User",
            "Status": 1
        };

    return Employee = await FetchRequest('GetEmployeeById', employee)
}
function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}




async function FetchRequest(uri, model) {
    console.log("Req sent");


    const token = localStorage.getItem("authToken");


    let headers = {
        'Content-Type': 'application/json'
    };


    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    try {
        const response = await fetch(host + '/' + uri, {
            method: 'POST',
            headers: headers,
            body: JSON.stringify(model)
        });

        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            throw new Error('Error performing operation');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation');
        return null;
    }
}
async function FetchRequestGET(uri) {
    try {
        const token = localStorage.getItem("authToken");

        let headers = {
            'Content-Type': 'application/json'
        };

        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        // Make the GET request
        const response = await fetch(host + '/' + uri, {
            method: 'GET',
            headers: headers
        });

        if (response.ok) {
            const data = await response.json();
            return data;
        } else {
            throw new Error('Error performing operation');
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Failed to perform the operation');
    }
}