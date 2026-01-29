<template>
  <q-layout view="lHh Lpr lFf" class="bg-dark text-white">
    <q-header v-if="showLoader" bordered class="bg-dark text-white" style="border-bottom: 1px solid rgba(255, 255, 255, 0.1)">
      <q-toolbar class="container q-mx-auto">
        <q-btn flat no-caps no-wrap dense to="/" class="q-mr-sm">
          <q-toolbar-title shrink class="text-weight-bold row items-center">
            <q-icon name="account_balance_wallet" class="q-mr-sm text-primary" size="32px" />
            CBDC-W
          </q-toolbar-title>
        </q-btn>

        <q-space />

        <div class="gt-xs q-gutter-x-lg text-weight-medium">
          <router-link to="/" class="nav-link">Home</router-link>
          <router-link to="/documentation" class="nav-link">Docs</router-link>
          <router-link to="/team" class="nav-link">Foundation</router-link>
        </div>

        <q-space />

        <div class="q-gutter-sm row items-center">
          <template v-if="!isLoggedIn">
            <q-btn flat label="Login" to="/auth/login" class="gt-xs text-grey-5 hover-white" no-caps />
            <q-btn unelevated color="primary" label="Get Started" to="/auth/register" class="q-px-lg shadow-glow-sm" rounded no-caps />
          </template>
          <template v-else>
            <q-btn flat label="Logout" @click="handleLogout" class="gt-xs text-grey-5 hover-white" no-caps />
            <q-btn unelevated color="primary" label="My Dashboard" to="/dashboard" class="q-px-lg shadow-glow-sm" rounded no-caps />
          </template>
        </div>
      </q-toolbar>
    </q-header>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
const isLoggedIn = ref(false)

const checkAuth = () => {
  isLoggedIn.value = !!localStorage.getItem('cbdc_user')
}

onMounted(checkAuth)

// Watch for route changes to refresh auth state (e.g. after login/logout navigation)
watch(() => route.path, checkAuth)

const showLoader = computed(() => !route.path.startsWith('/dashboard'))

function handleLogout() {
  localStorage.removeItem('cbdc_user')
  isLoggedIn.value = false
  router.push('/')
}
</script>

<style lang="scss" scoped>
.container {
  max-width: 1200px;
}
.nav-link {
  color: #9ca3af;
  text-decoration: none;
  transition: color 0.3s ease;
  &:hover {
    color: white;
  }
  &.router-link-active {
    color: #00d2ff;
  }
}
.hover-white:hover {
  color: white !important;
}
.shadow-glow-sm {
  box-shadow: 0 0 15px rgba(0, 210, 255, 0.3);
}
</style>
