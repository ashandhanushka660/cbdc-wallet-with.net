<template>
  <q-layout view="lHh LpR lFf" class="bg-dark text-white">
    <q-header
      bordered
      class="bg-dark text-white"
      style="border-bottom: 1px solid rgba(255, 255, 255, 0.1)"
    >
      <q-toolbar>
        <q-btn dense flat round icon="menu" @click="toggleLeftDrawer" />

        <q-btn flat no-caps no-wrap dense to="/" class="q-mr-sm">
          <q-toolbar-title shrink class="text-weight-bold">
            <q-icon name="account_balance_wallet" class="q-mr-sm text-primary" />
            My Wallet
          </q-toolbar-title>
        </q-btn>

        <q-space />

        <!-- Notifications -->
        <q-btn flat round dense icon="notifications" class="q-mr-sm">
          <q-badge floating color="red" rounded label="2" />
        </q-btn>

        <!-- Profile Dropdown -->
        <q-btn flat no-caps dense class="q-ml-sm">
          <div class="row items-center no-wrap">
            <div class="gt-xs text-weight-bold q-mr-sm">{{ userName }}</div>
            <q-avatar size="32px">
              <img src="https://cdn.quasar.dev/img/avatar.png">
            </q-avatar>
          </div>
          <q-menu dark auto-close>
            <q-list style="min-width: 150px">
              <q-item clickable>
                <q-item-section>Profile</q-item-section>
              </q-item>
              <q-separator dark />
              <q-item clickable class="text-red-4" @click="handleSignOut">
                <q-item-section>Sign Out</q-item-section>
              </q-item>
            </q-list>
          </q-menu>
        </q-btn>
      </q-toolbar>
    </q-header>

    <q-drawer
      show-if-above
      v-model="leftDrawerOpen"
      side="left"
      bordered
      dark
      class="bg-darker text-white"
      :width="260"
    >
      <div class="q-pa-md text-center">
        <div class="text-subtitle2 text-grey-5 q-mb-xs">Account ID</div>
        <div
          class="text-caption font-mono bg-surface q-pa-sm rounded-borders text-blue-2"
          style="border: 1px solid rgba(255, 255, 255, 0.1); overflow: hidden; text-overflow: ellipsis;"
        >
          {{ walletAddress || 'Loading...' }}
        </div>
      </div>

      <q-list padding>
        <q-item clickable v-ripple to="/dashboard" exact class="sidebar-item">
          <q-item-section avatar> <q-icon name="dashboard" /> </q-item-section>
          <q-item-section> Overview </q-item-section>
        </q-item>

        <q-item clickable v-ripple to="/dashboard/transactions" class="sidebar-item">
          <q-item-section avatar> <q-icon name="sync_alt" /> </q-item-section>
          <q-item-section> Transactions </q-item-section>
        </q-item>

        <q-item clickable v-ripple to="/dashboard/cards" class="sidebar-item">
          <q-item-section avatar> <q-icon name="credit_card" /> </q-item-section>
          <q-item-section> Cards & Accounts </q-item-section>
        </q-item>

        <q-item clickable v-ripple to="/dashboard/loans" class="sidebar-item">
          <q-item-section avatar> <q-icon name="payments" /> </q-item-section>
          <q-item-section> Loans & Credit </q-item-section>
        </q-item>

        <q-item clickable v-ripple to="/dashboard/services" class="sidebar-item">
          <q-item-section avatar> <q-icon name="hub" /> </q-item-section>
          <q-item-section> Services </q-item-section>
        </q-item>

        <q-item clickable v-ripple to="/dashboard/settings" class="sidebar-item">
          <q-item-section avatar> <q-icon name="settings" /> </q-item-section>
          <q-item-section> Settings </q-item-section>
        </q-item>

        <q-item clickable v-ripple to="/dashboard/admin" class="sidebar-item">
          <q-item-section avatar> <q-icon name="security" /> </q-item-section>
          <q-item-section> Institutional Command </q-item-section>
        </q-item>

        <q-separator dark spaced />

        <q-item clickable v-ripple class="sidebar-item text-red-4" @click="handleSignOut">
          <q-item-section avatar> <q-icon name="logout" /> </q-item-section>
          <q-item-section> Sign Out </q-item-section>
        </q-item>
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>

    <!-- Mobile Bottom Navigation -->
    <q-footer bordered class="bg-dark text-white lt-md" style="border-top: 1px solid rgba(255, 255, 255, 0.1)">
      <q-tabs no-caps active-color="primary" indicator-color="transparent" class="text-grey-5" v-model="tab">
        <q-route-tab to="/dashboard" name="dashboard" icon="dashboard" label="Home" exact />
        <q-route-tab to="/dashboard/transactions" name="transactions" icon="sync_alt" label="Activity" />
        <q-route-tab to="/dashboard/cards" name="cards" icon="credit_card" label="Cards" />
        <q-route-tab to="/dashboard/loans" name="loans" icon="payments" label="Loans" />
      </q-tabs>
    </q-footer>
  </q-layout>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'

const leftDrawerOpen = ref(false)
const tab = ref('dashboard')
const router = useRouter()
const $q = useQuasar()
const walletAddress = ref('')
const userName = ref('User')

onMounted(() => {
  const userData = localStorage.getItem('cbdc_user')
  if (userData) {
    const user = JSON.parse(userData)
    walletAddress.value = user.wallet?.walletAddress || user.id
    userName.value = user.firstName || 'User'
  }
})

function toggleLeftDrawer() {
  leftDrawerOpen.value = !leftDrawerOpen.value
}

function handleSignOut() {
  localStorage.removeItem('cbdc_user')
  $q.notify({
    color: 'green-4',
    textColor: 'white',
    icon: 'check_circle',
    message: 'Signed out successfully',
  })
  router.push('/auth/login')
}
</script>

<style scoped>
.bg-darker {
  background: #0a0a0b;
}
.bg-primary-soft {
  background: rgba(0, 210, 255, 0.15);
}
.bg-surface {
  background: rgba(255, 255, 255, 0.08);
}
</style>
