<template>
  <v-card>
    <v-data-table
      :headers="headers"
      :items="desserts"
      sort-by="calories"
      class="elevation-1"
      item-key="Id"
      show-select
      v-model="selected"
      :search="search"
    >
      <template v-slot:top>
        <!-- 搜索框 -->
        <v-toolbar flat>
          
          <v-spacer></v-spacer>
          <v-text-field
            v-model="search"
            append-icon="mdi-magnify"
            label="搜索"
            single-line
            hide-details
            class="mx-4"
          ></v-text-field>

          <!-- 设置特殊权限提示框 -->
          <v-dialog v-model="dialogAction" max-width="500px">
            <v-card>
              <v-card-title>
                <span class="headline">设置特殊权限</span>
              </v-card-title>

              <v-card-text>
                <v-container fluid>
                  <v-col cols="12">
                    <v-combobox
                      v-model="selectAction"
                      :items="itemsAction"
                      label="点击选择特殊权限"
                      multiple
                      chips
                    ></v-combobox>
                  </v-col>
                </v-container>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeAction">
                  取消
                </v-btn>
                <v-btn color="blue darken-1" text @click="saveAction">
                  保存
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
<!-- ---------------------------------------------------------------------------------------- -->
          <!-- 设置角色提示框 -->
          <v-dialog v-model="dialogRole" max-width="500px">
            <v-card>
              <v-card-title>
                <span class="headline">设置角色</span>
              </v-card-title>

              <v-card-text>
                <v-container fluid>
                  <v-col cols="12">
                    <v-combobox
                      v-model="selectRole"
                      :items="itemsRole"
                      label="点击选择角色"
                      multiple
                      chips
                    ></v-combobox>
                  </v-col>
                </v-container>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeRole">
                  取消
                </v-btn>
                <v-btn color="blue darken-1" text @click="saveRole">
                  保存
                </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
<!-- ------------------------------------------------------------------------- -->
          <!-- 添加提示框 -->
          <v-dialog v-model="dialog" max-width="500px">
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                color="primary"
                dark
                class="mb-2 mx-2"
                v-bind="attrs"
                v-on="on"
              >
                添加新项
              </v-btn>
            </template>
            <v-card>
              <v-card-title>
                <span class="headline">{{ formTitle }}</span>
              </v-card-title>

              <v-card-text>
                <v-container>
                  <v-row>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.user_name"
                        label="用户名"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.password"
                        label="密码"
                      ></v-text-field>
                    </v-col>
                    <!-- <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.fat"
                        label="Fat (g)"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.carbs"
                        label="Carbs (g)"
                      ></v-text-field>
                    </v-col>
                    <v-col cols="12" sm="6" md="4">
                      <v-text-field
                        v-model="editedItem.protein"
                        label="Protein (g)"
                      ></v-text-field>
                    </v-col> -->
                  </v-row>
                </v-container>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="close"> 取消 </v-btn>
                <v-btn color="blue darken-1" text @click="save"> 保存 </v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>

          <v-btn color="red" dark class="mb-2" @click="deleteAllItem()">
            删除所选
          </v-btn>

          <!-- 删除提示框 -->
          <v-dialog v-model="dialogDeleteAll" max-width="500px">
            <v-card>
              <v-card-title class="headline"
                >你确定要删除所选记录吗？</v-card-title
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeDeleteAll"
                  >取消</v-btn
                >
                <v-btn color="blue darken-1" text @click="deleteAllItemConfirm"
                  >确定</v-btn
                >
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>

          <!-- 删除提示框 -->
          <v-dialog v-model="dialogDelete" max-width="500px">
            <v-card>
              <v-card-title class="headline"
                >你确定要删除此条记录吗？</v-card-title
              >
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="blue darken-1" text @click="closeDelete"
                  >取消</v-btn
                >
                <v-btn color="blue darken-1" text @click="deleteItemConfirm"
                  >确定</v-btn
                >
                <v-spacer></v-spacer>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-toolbar>
      </template>

      <!-- 表格中的删除和修改 -->
      <!-- 设置角色和特殊权限 -->
      <template v-slot:item.actions="{ item }">
        <v-icon small class="mr-2" @click="editItem(item)"> mdi-pencil </v-icon>
        <v-icon small class="mr-2" @click="deleteItem(item)">
          mdi-delete
        </v-icon>
        <v-icon small class="mr-2" @click="setRoleItem(item)"> mdi-gavel </v-icon>

        <v-icon small @click="setActionItem(item)"> mdi-lock </v-icon>
      </template>

      <!-- 初始化 -->
      <template v-slot:no-data>
        <v-btn color="primary" @click="initialize"> 刷新 </v-btn>
      </template>
    </v-data-table>

    <!-- <template>
    <v-pagination
      v-model="page"
      :length="15"
      :total-visible="7"
    ></v-pagination>
          </template> -->
  </v-card>
</template>
<script>
import userApi from "@/api/userApi";
import roleApi from "@/api/roleApi";
import actionApi from "@/api/actionApi";
export default {
  data: () => ({
    allAction: [],
    selectAction: [],
    itemsAction: [],
    dialogAction: false,
// -------------------------------
    allRole: [],
    selectRole: [],
    itemsRole: [],
    dialogRole: false,
// -----------------------------

    page: 1,
    selected: [],
    search: "",
    dialog: false,
    dialogDelete: false,
    dialogDeleteAll: false,
    headers: [
      {
        text: "编号",
        align: "start",
        value: "Id",
      },
      { text: "用户名", value: "user_name", sortable: false },
      { text: "密码", value: "password", sortable: false },
      { text: "操作", value: "actions", sortable: false },
    ],
    desserts: [],
    editedIndex: -1,
    editedItem: {
      user_name: 0,
      password: 0,
    },
    defaultItem: {
      user_name: "",
      password: 0,
    },
  }),

  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "添加数据" : "编辑数据";
    },
  },

  watch: {
    dialog(val) {
      val || this.close();
    },
    dialogDelete(val) {
      val || this.closeDelete();
    },
  },

  created() {
    this.initialize();
  },

  methods: {
    initialize() {
      userApi.getAllUser().then((resp) => {
        const response=resp.data;
        this.desserts = response;
      });
    },
    // 添加和修改都打开同一个编辑框
    editItem(item) {
      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },
// ----------------------------------------------------------------------------------------
    setRoleItem(item) {
      //forEach中this指向已经发生变化
      //获取所有角色
      roleApi.getRoles().then((resp) => {
        const response=resp.data;
        this.allRole = response;
        this.itemsRole = [];

        for (let i = 0; i < response.length; i++) {
          this.itemsRole.push(response[i].role_name);
        }
      });

      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      //获取已经存在的角色
      userApi.getRoleByuserId(this.editedItem.Id).then((resp) => {
        const response=resp.data;
        for (let i = 0; i < response.length; i++) {
          this.selectRole.push(response[i].role_name);
        }
      });
      this.dialogRole = true;
    },
    closeRole() {
      this.dialogRole = false;
      this.selectRole = [];
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    saveRole() {
      var Ids = [];
      const myAllRoles = this.allRole;
      for (let i = 0; i < myAllRoles.length; i++) {
        var resule = this.selectRole.find(function (item) {
          return item == myAllRoles[i].role_name;
        });
        if (resule) {
          Ids.push(myAllRoles[i].Id);
        }
      }
      userApi.setRole(this.editedItem.Id, Ids).then(() => {
        this.closeRole();
      });
    },
    // -----------------------------------------------------------------------------------------
    // --------------------------------------------------------------------------------------------
    setActionItem(item) {
      //forEach中this指向已经发生变化
      //获取所有权限
      actionApi.getActions().then((resp) => {
        const response=resp.data;
        this.allAction = response;
        this.itemsAction = [];

        for (let i = 0; i < response.length; i++) {
          this.itemsAction.push(response[i].action_name);
        }
      });

      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      //获取已经存在的特殊权限
      userApi.getSpecialAction(this.editedItem.Id).then((resp) => {
        const response=resp.data;
        for (let i = 0; i < response.length; i++) {
          this.selectAction.push(response[i].action_name);
        }
      });
      this.dialogAction = true;
    },
    closeAction() {
      this.dialogAction = false;
      this.selectAction = [];
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    saveAction() {
      var Ids = [];
      const myAllAction = this.allAction;
      for (let i = 0; i < myAllAction.length; i++) {
        var resule = this.selectAction.find(function (item) {
          return item == myAllAction[i].action_name;
        });
        if (resule) {
          Ids.push(myAllAction[i].Id);
        }
      }
      userApi.setSpecialAction(this.editedItem.Id, Ids).then(() => {
        this.closeAction();
      });
    },

    // ----------------------------------------------------------------------------------------------

    deleteItem(item) {
      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },
    deleteAllItem() {
      if (this.selected.length > 0) {
        this.dialogDeleteAll = true;
      }
    },
    deleteItemConfirm() {
      userApi.delUser(this.editedItem.Id).then(() => this.initialize());
      this.closeDelete();
    },
    deleteAllItemConfirm() {
      var Ids = [];
      this.selected.forEach(function (item) {
        Ids.push(item.Id);
      });
      userApi.delAllUser(Ids).then(() => this.initialize());
      this.closeDeleteAll();
    },
    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },

    closeDelete() {
      this.dialogDelete = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },
    closeDeleteAll() {
      this.dialogDeleteAll = false;
    },
    save() {
      if (this.editedIndex > -1) {
        userApi.updateUser(this.editedItem).then(() => this.initialize());
      } else {
        userApi.addUser(this.editedItem).then(() => this.initialize());
      }
      this.close();
    },
  },
};
</script>