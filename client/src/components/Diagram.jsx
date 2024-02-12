
// https://datalanguage.com/blog/graphical-uis-with-svg-and-react-part-1-declarative-graphics
import PropTypes from 'prop-types';
import { useDataStreamContext } from '../providers/DataStreamProvider';

const basicProps = {
    x: PropTypes.number.isRequired,
    y: PropTypes.number.isRequired,
    width: PropTypes.number,
    height: PropTypes.number,
}

const LabelledBlock = ({ deviceState, x, y, width, height }) => {
    const textWidth = width ?? 20;
    const textHeight = height ?? 20;

    if (deviceState == null) {
        return;
    }

    const valueFontSize = 16;
    const smallLabelFontSize = 10;
    const smallExtremumFontSize = 12;
    const extremumPadding = 4;
    return (
        <g>
            <text x={x} y={y - extremumPadding} textAnchor='left' dominantBaseline='auto' fontSize={12}>{deviceState.name}</text>

            <rect x={x} y={y} width={textWidth} height={textHeight} rx="10" ry="10" style={{ "fill": "white", "stroke": "black" }} />
            <text x={x + textWidth / 2} y={y + textHeight / 2} textAnchor='middle' dominantBaseline='middle' fontWeight={"bold"} fontSize={valueFontSize}>{deviceState.value}</text>

            <text x={x} y={y + textHeight + extremumPadding} textAnchor='start' dominantBaseline='hanging' fontSize={smallLabelFontSize}>MIN</text>
            <text x={x} y={y + textHeight + smallLabelFontSize + extremumPadding} textAnchor='start' dominantBaseline='hanging' fontSize={smallExtremumFontSize}>{deviceState.valueRunMinimum}</text>

            <text x={x + textWidth} y={y + textHeight + extremumPadding} textAnchor='end' dominantBaseline='hanging' fontSize={smallLabelFontSize}>MAX</text>
            <text x={x + textWidth} y={y + textHeight + smallLabelFontSize +extremumPadding} textAnchor='end' dominantBaseline='hanging' fontSize={smallExtremumFontSize}>{deviceState.valueRunMaximum}</text>
        </g>
    )
}

LabelledBlock.propTypes = {
    ...basicProps,
    deviceState: PropTypes.object.isRequired,
};

const FixedDevices = [
    {
        drawingId: "Temp1",
        location: {
            x: 160,
            y: 160
        }
    },
    {
        drawingId: "Pres1",
        location: {
            x: 300,
            y: 420
        }
    },
]

const Diagram = () => {

    const dataStreamContext = useDataStreamContext();
    const width = 800;
    const height = 600;

    const drawingIds = FixedDevices.map(fixedDevice => fixedDevice.drawingId);
    const drawingDevices = {}
    for (const [deviceId, deviceState] of Object.entries(dataStreamContext.deviceStates)) {
        if (drawingIds.includes(deviceState.drawingId)) {
            drawingDevices[deviceState.drawingId] = deviceState;
        }
    }
    return (
        <div >
            <svg width={width} height={height} viewBox={`0 0 ${width} ${height}`} style={{ border: "2px solid gray", borderRadius: 8 }}>
                <circle cx={40} cy={40} r={30} fill="red" />
                {FixedDevices.map((device) => {
                    let deviceState = null;
                    if (device.drawingId in drawingDevices) {
                        deviceState = drawingDevices[device.drawingId];
                    }
                    return (<LabelledBlock key={device.drawingId}
                        width={80}
                        height={40}
                        deviceState={deviceState}
                        x={device.location.x}
                        y={device.location.y}
                    />);

                })}
            </svg>
        </div>
    )

}


export default Diagram;