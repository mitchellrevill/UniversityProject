async function fetchApiRequest(functionName) {
    try {
        var response = await fetch(host + '/' + functionName);
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        console.log(response);

        var jsonResponse = await response.json();
        console.log('apiRequest function successful:', jsonResponse);
        switch (functionName) {
            case "GetAllEmployees":
                displayEmployees(jsonResponse);
                break;
        }

    } catch (error) {
        console.error('Failed to fetch employees:', error);
        alert('Failed to load employee data. Please check your connection.');
    }
}

function displayEmployees(employees) {
    const tbody = document.querySelector('#jsonTable tbody');

    if (!tbody) {
        console.error('Table body not found');
        return;
    }

    tbody.innerHTML = ''; // Clear any existing rows

    employees.forEach(employee => {
        const row = document.createElement('tr');

        row.innerHTML = `
            <td>${employee.EmployeeId}</td>
            <td>${employee.FirstName}</td>
            <td>${employee.LastName}</td>
            <td>${employee.CompanyEmail}</td>
            <td>${employee.PersonalEmail}</td>
        `;
        tbody.appendChild(row);
    });
}