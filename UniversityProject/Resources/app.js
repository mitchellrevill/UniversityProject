// Load employees from storage when the Employees page loads
function loadEmployeesFromStorage() {
    const employeesData = localStorage.getItem('employeesData');

    if (employeesData) {
        const employees = JSON.parse(employeesData);
        displayEmployees(employees); // Call the display function with the parsed data
    } else {
        console.error('No employee data found in localStorage');
        alert('Failed to load employee data. Please try again.');
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
        const tr = document.createElement('tr');

        // Populate the table row with employee data
        const empIdTd = document.createElement('td');
        empIdTd.innerText = employee.EmployeeId;
        tr.appendChild(empIdTd);

        const firstNameTd = document.createElement('td');
        firstNameTd.innerText = employee.FirstName;
        tr.appendChild(firstNameTd);

        const lastNameTd = document.createElement('td');
        lastNameTd.innerText = employee.LastName;
        tr.appendChild(lastNameTd);

        const companyEmailTd = document.createElement('td');
        companyEmailTd.innerText = employee.CompanyEmail;
        tr.appendChild(companyEmailTd);

        const personalEmailTd = document.createElement('td');
        personalEmailTd.innerText = employee.PersonalEmail;
        tr.appendChild(personalEmailTd);

        tbody.appendChild(tr);
    });
}

// Trigger loading of employees when employeesPage loads
window.addEventListener('load', loadEmployeesFromStorage);
