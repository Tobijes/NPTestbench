using NPTestbench.Models;

public class ByteValue {
    public DeviceDataType dataType;
    public byte[] bytes;

    public ByteValue (DeviceDataType dataType, byte[] bytes) {
        this.dataType = dataType;
        this.bytes = bytes;
    }

    public float AsFloat () {
        if (dataType == DeviceDataType.Float32) {
            return  BitConverter.ToSingle(bytes);
        } else if (dataType == DeviceDataType.UInt16) {
            return BitConverter.ToUInt16(bytes);
        }
        return 0.0f;
    }

    public ushort AsUshort () {
        if (dataType == DeviceDataType.UInt16) {
            return BitConverter.ToUInt16(bytes);
        }
        return 0;
    }
}