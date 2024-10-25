/*
// Add event listener to load employee data when the button is clicked
document.querySelector('#loadEmployeesButton').addEventListener('click', () => {
    loadEmployees('http://localhost:8000/GetAllEmployees', '#employeeJsonTable');
});
*/

async function loadEmployees(url, tableSelector) {
    try {
        const response = await fetch(url);

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const employees = await response.json();
        console.log('Employees successfully loaded');
        displayEmployees(employees, tableSelector);
    } catch (error) {
        console.error('Failed to load employees:', error);
        alert('Failed to load employee data. Please check your connection.');
    }
}

function displayEmployees(employees, tableSelector) {
    const tbody = document.querySelector(`${tableSelector} tbody`);

    if (!tbody) {
        console.error('Table body not found');
        return;
    }

    tbody.innerHTML = ''; // Clear existing rows

    employees.forEach(employee => {
        const tr = document.createElement('tr');

        tr.innerHTML = `
            <td>${employee.EmployeeId}</td>
            <td>${employee.FirstName}</td>
            <td>${employee.LastName}</td>
            <td>${employee.CompanyEmail}</td>
            <td>${employee.PersonalEmail}</td>
        `;

        tbody.appendChild(tr);
    });
}
