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
populateLeaveRequest()




async function populateLeaveRequest() {
    try {
        const leaveRequests = await FetchRequestGET("GetLeaveRequest");
        if (!Array.isArray(leaveRequests)) {
            throw new Error('Expected an array from FetchRequestGET.');
        }

        
        const filteredRequests = leaveRequests.filter(request => request.EmployeeId === payload.EmployeeId);


        const tbody = document.getElementById('leaveRequestsTableBody');
        tbody.innerHTML = '';

        if (filteredRequests.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4">No locations available.</td></tr>';
            return;
        }

        filteredRequests.forEach(item => {
            const row = document.createElement('tr');

            row.innerHTML = `
                <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.EmployeeId}" data-name="${item.EmployeeId}" data-description="${item.Description}"></td>
                <td>${item.LeaveRequestId}</td>
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







async function populateEmployeeRequest() {
    try {
        const leaveRequests = await FetchRequestGET("GetLeaveRequest");
        
        if (!Array.isArray(leaveRequests)) {
            throw new Error('Expected an array from FetchRequestGET.');
        }

        const result = managers.reduce((acc, manager) => {
            if (payload.EmployeeId === manager.EmployeeId) {
                acc.isManager = true;
                acc.managerId = manager.EmployeeId;  // Store the manager's EmployeeId
            }
            return acc;
        }, { isManager: false, managerId: null });

            
        if (result.isManager) {
            const Employees = await FetchRequestGET("GetAllEmployees");


            const employeesUnderManager = Employees.filter(employee => employee.ManagerId === result.managerId);


            console.log('Employees under this manager:', employeesUnderManager);

        } else {
            
            return
        }

        const tbody = document.getElementById('EmployeeleaveRequestsTableBody');
        tbody.innerHTML = '';

        if (employeesUnderManager.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4">No locations available.</td></tr>';
            return;
        }

        employeesUnderManager.forEach(item => {
            const row = document.createElement('tr');

            row.innerHTML = `
                <td><input type="checkbox" class="dynamic-checkbox-item" data-id="${item.EmployeeId}" data-name="${item.EmployeeId}" data-description="${item.Description}"></td>
                <td>${item.LeaveRequestId}</td>
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
    const leaveDetails = await calculateLeave();
    var employee = await GetEmployee();

    console.log(employee)
    if (leaveDetails) {
        const { totalHoursUsed, hoursLeft } = leaveDetails; // Destructure the object
        console.log(`Total Hours Used: ${totalHoursUsed}`);
        console.log(`Hours Left: ${hoursLeft}`);

        
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
async function calculateLeave() {
    try {
        const leaveRequests = await FetchRequestGET("GetLeaveRequest");
        const Countries = await FetchRequestGET('GetCountries');
        const country = Countries.find((c) => String(c.CountryId) === String(Employee.CountryId));


        const employeeId = payload.EmployeeId;

        console.log(employeeId)
        const filteredRequests = leaveRequests.filter(request => request.EmployeeId === employeeId);

        let totalHoursUsed = 0;
        const currentYear = new Date().getFullYear();

        for (const request of filteredRequests) {
            const startDate = new Date(request.StartDate);
            if (startDate.getFullYear() === currentYear) {
                totalHoursUsed += request.HoursUsed || 0; // Add HoursUsed if the year matches
            }
        }

        let remainingLeave = country.MinimumLeave * 24;

        let hoursLeft =  remainingLeave - totalHoursUsed

            
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