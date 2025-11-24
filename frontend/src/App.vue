<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router';
import { useAuthStore } from '@/stores/auth';
import { useCartStore } from '@/stores/cart';
import { computed } from 'vue';

const authStore = useAuthStore();
const cartStore = useCartStore();

const isAuthenticated = computed(() => authStore.isAuthenticated());

function logout() {
  authStore.clearToken();
  window.location.href = '/';
}
</script>

<template>
  <div class="app-container">
    <!-- Navigation Header -->
    <header class="main-header">
      <div class="container">
        <div class="header-content">
          <!-- Logo -->
          <RouterLink to="/" class="logo">
            <svg
              class="logo-icon"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
            >
              <path d="M3 3h18v18H3z" />
              <path d="M9 9h6v6H9z" />
            </svg>
            <span class="logo-text">FastFood</span>
          </RouterLink>

          <!-- Main Navigation -->
          <nav class="main-nav">
            <RouterLink to="/" class="nav-link">Home</RouterLink>
            <RouterLink to="/menu" class="nav-link">Menu</RouterLink>
            <RouterLink to="/search" class="nav-link">Search</RouterLink>
            <RouterLink to="/about" class="nav-link">About</RouterLink>
          </nav>

          <!-- Right Side Actions -->
          <div class="header-actions">
            <!-- Cart Icon -->
            <RouterLink to="/cart" class="cart-button">
              <svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z"
                />
              </svg>
              <span v-if="cartStore.totalItems > 0" class="cart-badge">{{
                cartStore.totalItems
              }}</span>
            </RouterLink>

            <!-- User Menu -->
            <div v-if="isAuthenticated" class="user-menu">
              <RouterLink to="/profile" class="user-button">
                <svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor">
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    stroke-width="2"
                    d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"
                  />
                </svg>
                <span class="user-text">Profile</span>
              </RouterLink>
              <button @click="logout" class="logout-button">Logout</button>
            </div>

            <!-- Auth Buttons -->
            <div v-else class="auth-buttons">
              <RouterLink to="/login" class="btn-secondary">Login</RouterLink>
              <RouterLink to="/register" class="btn-primary">Sign Up</RouterLink>
            </div>
          </div>
        </div>
      </div>
    </header>

    <!-- Main Content -->
    <main class="main-content">
      <RouterView />
    </main>

    <!-- Footer -->
    <footer class="main-footer">
      <div class="container">
        <div class="footer-content">
          <div class="footer-section">
            <h3>FastFood</h3>
            <p>Delicious food delivered to your door</p>
          </div>
          <div class="footer-section">
            <h4>Quick Links</h4>
            <ul>
              <li><RouterLink to="/menu">Menu</RouterLink></li>
              <li><RouterLink to="/about">About Us</RouterLink></li>
              <li><RouterLink to="/search">Search</RouterLink></li>
            </ul>
          </div>
          <div class="footer-section">
            <h4>Account</h4>
            <ul>
              <template v-if="isAuthenticated">
                <li><RouterLink to="/profile">My Profile</RouterLink></li>
              </template>
              <template v-else>
                <li><RouterLink to="/login">Login</RouterLink></li>
                <li><RouterLink to="/register">Sign Up</RouterLink></li>
              </template>
            </ul>
          </div>
          <div class="footer-section">
            <h4>Contact</h4>
            <p>Email: info@fastfood.com</p>
            <p>Phone: (555) 123-4567</p>
          </div>
        </div>
        <div class="footer-bottom">
          <p>&copy; 2024 FastFood. All rights reserved.</p>
        </div>
      </div>
    </footer>
  </div>
</template>

<style scoped>
.app-container {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

/* Header Styles */
.main-header {
  background: white;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  position: sticky;
  top: 0;
  z-index: 100;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 1rem;
}

.header-content {
  display: flex;
  align-items: center;
  justify-content: space-between;
  height: 70px;
}

/* Logo */
.logo {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  text-decoration: none;
  color: #2563eb;
  font-weight: 700;
  font-size: 1.5rem;
}

.logo-icon {
  width: 32px;
  height: 32px;
}

.logo-text {
  display: none;
}

@media (min-width: 640px) {
  .logo-text {
    display: inline;
  }
}

/* Navigation */
.main-nav {
  display: none;
  gap: 2rem;
}

@media (min-width: 768px) {
  .main-nav {
    display: flex;
  }
}

.nav-link {
  text-decoration: none;
  color: #4b5563;
  font-weight: 500;
  transition: color 0.2s;
  position: relative;
}

.nav-link:hover {
  color: #2563eb;
}

.nav-link.router-link-active {
  color: #2563eb;
}

.nav-link.router-link-active::after {
  content: '';
  position: absolute;
  bottom: -8px;
  left: 0;
  right: 0;
  height: 2px;
  background: #2563eb;
}

/* Header Actions */
.header-actions {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.cart-button {
  position: relative;
  padding: 0.5rem;
  color: #4b5563;
  transition: color 0.2s;
  text-decoration: none;
}

.cart-button:hover {
  color: #2563eb;
}

.icon {
  width: 24px;
  height: 24px;
}

.cart-badge {
  position: absolute;
  top: 0;
  right: 0;
  background: #ef4444;
  color: white;
  font-size: 0.75rem;
  font-weight: 600;
  padding: 0.125rem 0.375rem;
  border-radius: 9999px;
  min-width: 20px;
  text-align: center;
}

/* User Menu */
.user-menu {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.user-button {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1rem;
  color: #4b5563;
  text-decoration: none;
  border-radius: 0.5rem;
  transition: background-color 0.2s;
}

.user-button:hover {
  background: #f3f4f6;
}

.user-text {
  display: none;
}

@media (min-width: 640px) {
  .user-text {
    display: inline;
  }
}

.logout-button {
  padding: 0.5rem 1rem;
  background: transparent;
  border: 1px solid #e5e7eb;
  border-radius: 0.5rem;
  color: #4b5563;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.logout-button:hover {
  background: #f3f4f6;
  border-color: #d1d5db;
}

/* Auth Buttons */
.auth-buttons {
  display: flex;
  gap: 0.5rem;
}

.btn-secondary,
.btn-primary {
  padding: 0.5rem 1rem;
  border-radius: 0.5rem;
  font-weight: 500;
  text-decoration: none;
  transition: all 0.2s;
}

.btn-secondary {
  color: #4b5563;
  border: 1px solid #e5e7eb;
}

.btn-secondary:hover {
  background: #f3f4f6;
}

.btn-primary {
  background: #2563eb;
  color: white;
}

.btn-primary:hover {
  background: #1d4ed8;
}

/* Main Content */
.main-content {
  flex: 1;
}

/* Footer */
.main-footer {
  background: #1f2937;
  color: #d1d5db;
  margin-top: 4rem;
}

.footer-content {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 2rem;
  padding: 3rem 0;
}

.footer-section h3,
.footer-section h4 {
  color: white;
  margin-bottom: 1rem;
}

.footer-section p {
  margin: 0.5rem 0;
}

.footer-section ul {
  list-style: none;
  padding: 0;
  margin: 0;
}

.footer-section ul li {
  margin: 0.5rem 0;
}

.footer-section a {
  color: #d1d5db;
  text-decoration: none;
  transition: color 0.2s;
}

.footer-section a:hover {
  color: white;
}

.footer-bottom {
  border-top: 1px solid #374151;
  padding: 1.5rem 0;
  text-align: center;
}

.footer-bottom p {
  margin: 0;
  color: #9ca3af;
}
</style>
