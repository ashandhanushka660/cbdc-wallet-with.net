<template>
  <q-page class="bg-dark-bg flex flex-center">
    <div class="login-box fade-in-up">
      <div class="text-center q-mb-xl">
        <div class="row items-center justify-center q-mb-md">
          <q-icon name="account_balance_wallet" size="48px" class="text-gradient q-mr-sm" />
          <h2 class="text-h3 text-weight-bolder text-white q-my-none">CBDC</h2>
        </div>
        <p class="text-grey-5 text-h6 text-weight-light">Enter your credentials to access your wallet</p>
      </div>

      <q-card class="wallet-card shadow-24 q-pa-lg">
        <q-form @submit="handleLogin" class="q-gutter-y-lg">
          <div>
            <label class="text-grey-5 text-caption q-mb-xs block">Email Address</label>
            <q-input
              v-model="email"
              placeholder="name@company.com"
              filled
              dark
              class="input-field"
              :rules="[val => !!val || 'Email is required']"
            >
              <template v-slot:prepend>
                <q-icon name="email" color="grey-6" />
              </template>
            </q-input>
          </div>

          <div>
            <label class="text-grey-5 text-caption q-mb-xs block">Secure Password</label>
            <q-input
              v-model="password"
              :type="showPwd ? 'text' : 'password'"
              placeholder="••••••••"
              filled
              dark
              class="input-field"
              :rules="[val => !!val || 'Password is required']"
            >
              <template v-slot:prepend>
                <q-icon name="lock" color="grey-6" />
              </template>
              <template v-slot:append>
                <q-icon
                  :name="showPwd ? 'visibility' : 'visibility_off'"
                  class="cursor-pointer"
                  color="grey-6"
                  @click="showPwd = !showPwd"
                />
              </template>
            </q-input>
          </div>

          <div class="row items-center justify-between">
            <q-checkbox v-model="remember" label="Remember me" dark dense color="primary" />
            <a href="#" class="text-primary text-caption hover-underline">Forgot Password?</a>
          </div>

          <q-btn
            type="submit"
            label="Sign In"
            class="full-width btn-primary q-py-md text-h6"
            no-caps
            unelevated
            rounded
            :loading="loading"
          />

          <div class="text-center q-mt-md">
            <span class="text-grey-5 text-caption">Don't have an account? </span>
            <router-link to="/register" class="text-primary text-caption text-weight-bold">Register Now</router-link>
          </div>
        </q-form>
      </q-card>
    </div>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'

const router = useRouter()
const $q = useQuasar()

const email = ref('')
const password = ref('')
const showPwd = ref(false)
const remember = ref(false)
const loading = ref(false)

async function handleLogin() {
  loading.value = true

  try {
    const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5005'
    const response = await fetch(`${API_URL}/api/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        email: email.value,
        password: password.value
      })
    })

    const data = await response.json()

    if (data.success) {
      $q.notify({
        message: 'Login Successful',
        color: 'positive',
        icon: 'check_circle'
      })

      // Save user data for dashboard
      localStorage.setItem('cbdc_user', JSON.stringify(data.user))

      router.push('/dashboard')
    } else {
      $q.notify({
        message: data.message || 'Login failed',
        color: 'negative',
        icon: 'error',
        position: 'top'
      })
    }
  } catch (error) {
    console.error('Login error:', error)
    $q.notify({
      message: 'Failed to connect to server. Please ensure the backend is running.',
      color: 'negative',
      icon: 'wifi_off',
      position: 'top'
    })
  } finally {
    loading.value = false
  }
}
</script>

<style scoped lang="scss">
.login-box {
  width: 100%;
  max-width: 450px;
  padding: 20px;
}

.bg-dark-bg {
  background: #0a0e27;
}

.hover-underline:hover {
  text-decoration: underline;
}
</style>
