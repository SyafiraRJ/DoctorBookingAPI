# Doctor Booking API

A comprehensive RESTful API for managing doctor appointments, built with .NET Core. This API provides endpoints for user management, doctor profiles, appointment scheduling, reviews, and healthcare blog management.

## 📋 Table of Contents

- [Base URL & Swagger](#base-url--swagger)
- [Image Handling](#image-handling)
- [Entities / Models](#entities--models)
- [Data Transfer Objects (DTOs)](#data-transfer-objects-dtos)
- [API Endpoints](#api-endpoints)
- [Example Requests](#example-requests)
- [Installation & Setup](#installation--setup)
- [Database](#database)

---

## 🔗 Base URL & Swagger

- Base URL: `https://localhost:7031`
- Swagger Documentation: `https://localhost:7031/swagger/v1/swagger.json`
- Swagger UI: `https://localhost:7031/swagger/index.html`

---

## 🖼️ Image Handling

The `imageUrl` field stores image paths relative to the `wwwroot` folder (e.g., `'images/blogs/example.jpg'`).

To display images, use: **Base URL + imageUrl**

Examples:
- Specialization image: `https://localhost:7031/images/specializations/dentist.jpeg`
- Doctor photo: `https://localhost:7031/images/doctors/dr_patricia_ahoy.jpeg`

---

## 🗂️ Entities / Models

- **Users** – User account data
- **Specializations** – Medical specializations (e.g., "General Practitioner", "Dentist")
- **Doctors** – Doctor profiles including name, specialization, provider, and related info
- **Providers** – Healthcare providers (clinics, hospitals)
- **DoctorSchedules** – Doctor practice schedules with available time slots
- **Appointments** – Appointment data between users and doctors
- **Blogs** – Health information articles/blogs
- **Reviews** – User reviews for doctors

---

## 🗄️ Database

Database scripts and setup files are available in the `resources/Database` directory.

### Key Features:
- **Queue System**: Automatic queue number generation for appointments
- **Soft Delete**: Records are deactivated (IsActive = false) instead of being permanently deleted
- **Calculated Fields**: Doctor ratings and review counts are calculated dynamically
- **Time Slot Management**: Automatic validation for appointment scheduling conflicts

---

## 🤝 Contributing

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 
Project Link:  https://github.com/SyafiraRJ/DoctorBookingAPI.git